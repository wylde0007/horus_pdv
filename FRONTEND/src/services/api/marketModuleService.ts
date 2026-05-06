import type { MarketModuleConfig } from "@/components/Admin/MarketModulePage";
import { apiRequest } from "./apiClient";

const MODULOS_MERCADO_API_URL =
  import.meta.env.VITE_MODULOS_MERCADO_API_URL ?? "http://localhost:5260/api/ModuloMercado";

export const marketModuleService = {
  async get(id: string) {
    const response = await apiRequest<MarketModuleConfig>(`${MODULOS_MERCADO_API_URL}/${id}`);
    return response.data;
  },
};
