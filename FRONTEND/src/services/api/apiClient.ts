/**
 * Arquivo: src/services/api/apiClient.ts
 * Objetivo: centralizar chamadas HTTP para a API .NET do Hórus PDV.
 */
export type ApiResponse<T> = {
  success: boolean;
  message: string;
  details?: string;
  data?: T;
};

export async function apiRequest<T>(
  endpointUrl: string,
  options: RequestInit = {},
): Promise<ApiResponse<T>> {
  const response = await fetch(endpointUrl, {
    headers: {
      "Content-Type": "application/json",
      ...options.headers,
    },
    ...options,
  });

  const payload = (await response.json()) as ApiResponse<T>;
  if (!response.ok || !payload.success) {
    throw new Error(payload.message || "Erro ao comunicar com a API.");
  }

  return payload;
}
