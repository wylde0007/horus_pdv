import { apiRequest } from "./apiClient";

const HISTORICO_VENDAS_API_URL =
  import.meta.env.VITE_HISTORICO_VENDAS_API_URL ?? "http://localhost:5260/api/HistoricoVendas";

export type SaleHistoryDto = {
  saleNumber: string;
  customerName: string;
  customerCpf: string;
  productCode: string;
  productName: string;
  quantity: number;
  saleDate: string;
};

export const salesHistoryService = {
  async list() {
    const response = await apiRequest<SaleHistoryDto[]>(HISTORICO_VENDAS_API_URL);
    return response.data ?? [];
  },
};
