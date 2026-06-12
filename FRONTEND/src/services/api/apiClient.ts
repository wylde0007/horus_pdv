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

export async function apiRequest<T>(
  endpointUrl: string,
  options: ApiRequestOptions = {},
): Promise<ApiResponse<T>> {
  const { skipAuth = false, headers, ...requestOptions } = options;

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

  const response = await fetch(endpointUrl, {
    credentials: "include",
    headers: requestHeaders,
    ...requestOptions,
  });

  const contentType = response.headers.get("content-type") || "";
  const payload = contentType.includes("application/json")
    ? ((await response.json()) as ApiResponse<T>)
    : ({
        success: response.ok,
        message: response.ok ? "Operação concluída." : "Erro ao comunicar com a API.",
      } as ApiResponse<T>);

  if (response.status === 401 && !skipAuth) {
    clearAuthSession();
  }

  if (!response.ok || !payload.success) {
    throw new Error(payload.message || "Erro ao comunicar com a API.");
  }

  return payload;
}
