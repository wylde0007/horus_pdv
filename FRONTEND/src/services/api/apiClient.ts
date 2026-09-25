/**
 * Arquivo: src/services/api/apiClient.ts
 * Objetivo: centralizar chamadas HTTP para a API .NET do Hórus PDV.
 * Entradas esperadas: recebe caminho, método e payload opcional para executar requisições autenticadas.
 */
import { clearAuthSession, getStoredAuthToken } from "@/utils/authStorage";

export type ApiResponse<T> = {
  success: boolean;
  message: string;
  details?: string;
  data?: T;
};

type ApiRequestOptions = RequestInit & {
  skipAuth?: boolean;
};

const PRIMARY_API_ORIGIN = "https://horus-pdv.onrender.com";
const FALLBACK_API_ORIGIN = "https://sales-crm-bj9z.onrender.com";

let activeApiOrigin = PRIMARY_API_ORIGIN;

const FAILOVER_STATUS_CODES = new Set([502, 503, 504]);

function getRequestMethod(options: ApiRequestOptions): string {
  return (options.method ?? "GET").toUpperCase();
}

function isSafeToRetry(method: string): boolean {
  return method === "GET" || method === "HEAD";
}

function buildApiUrl(endpointUrl: string, origin: string): string {
  try {
    const parsed = new URL(endpointUrl);

    return `${origin}${parsed.pathname}${parsed.search}${parsed.hash}`;
  } catch {
    const path = endpointUrl.startsWith("/") ? endpointUrl : `/${endpointUrl}`;
    return `${origin}${path}`;
  }
}

async function executeRequest<T>(
  endpointUrl: string,
  origin: string,
  options: ApiRequestOptions,
  requestHeaders: Headers,
): Promise<{
  response: Response;
  payload: ApiResponse<T>;
}> {
  const url = buildApiUrl(endpointUrl, origin);

  const { skipAuth: _skipAuth, headers: _headers, ...requestOptions } = options;

  const response = await fetch(url, {
    credentials: "include",
    headers: requestHeaders,
    ...requestOptions,
  });

  const contentType = response.headers.get("content-type") || "";

  const payload = contentType.includes("application/json")
    ? ((await response.json()) as ApiResponse<T>)
    : ({
        success: response.ok,
        message: response.ok
          ? "Operação concluída."
          : "Erro ao comunicar com a API.",
      } as ApiResponse<T>);

  return {
    response,
    payload,
  };
}

export async function apiRequest<T>(
  endpointUrl: string,
  options: ApiRequestOptions = {},
): Promise<ApiResponse<T>> {
  const { skipAuth = false, headers } = options;

  const requestHeaders = new Headers(headers);

  if (!requestHeaders.has("Content-Type")) {
    requestHeaders.set("Content-Type", "application/json");
  }

  if (!skipAuth) {
    const token = getStoredAuthToken();

    if (token) {
      requestHeaders.set("Authorization", `Bearer ${token}`);
    }
  }

  const method = getRequestMethod(options);
  const safeToRetry = isSafeToRetry(method);

  let currentOrigin = activeApiOrigin;

  try {
    let result = await executeRequest<T>(
      endpointUrl,
      currentOrigin,
      options,
      requestHeaders,
    );

    if (
      currentOrigin === PRIMARY_API_ORIGIN &&
      safeToRetry &&
      FAILOVER_STATUS_CODES.has(result.response.status)
    ) {
      console.warn(
        "[API] Backend principal indisponível. Alterando para o fallback.",
      );

      activeApiOrigin = FALLBACK_API_ORIGIN;
      currentOrigin = FALLBACK_API_ORIGIN;

      result = await executeRequest<T>(
        endpointUrl,
        currentOrigin,
        options,
        requestHeaders,
      );
    }

    const { response, payload } = result;

    if (response.status === 401 && !skipAuth) {
      clearAuthSession();
    }

    if (!response.ok || !payload.success) {
      throw new Error(payload.message || "Erro ao comunicar com a API.");
    }

    return payload;
  } catch (error) {
    if (
      currentOrigin === PRIMARY_API_ORIGIN &&
      safeToRetry &&
      error instanceof TypeError
    ) {
      console.warn(
        "[API] Falha de rede no backend principal. Alterando para o fallback.",
      );

      activeApiOrigin = FALLBACK_API_ORIGIN;

      const { response, payload } = await executeRequest<T>(
        endpointUrl,
        FALLBACK_API_ORIGIN,
        options,
        requestHeaders,
      );

      if (response.status === 401 && !skipAuth) {
        clearAuthSession();
      }

      if (!response.ok || !payload.success) {
        throw new Error(payload.message || "Erro ao comunicar com a API.");
      }

      return payload;
    }

    throw error;
  }
}
