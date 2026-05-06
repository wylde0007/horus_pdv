import { apiRequest } from "./apiClient";

export type SupplierDto = {
  id: string;
  companyName: string;
  fantasyName: string;
  cnpj: string;
  cep: string;
  city: string;
  state: string;
  address: string;
  neighborhood: string;
  streetComplement: string;
  number: string;
  referencePoint: string;
  telephone: string;
  cellphone: string;
  email: string;
};

export type SupplierPayload = Omit<SupplierDto, "id">;

export const supplierService = {
  async list() {
    const response = await apiRequest<SupplierDto[]>("/Fornecedor");
    return response.data ?? [];
  },
  async create(payload: SupplierPayload) {
    const response = await apiRequest<SupplierDto>("/Fornecedor", {
      method: "POST",
      body: JSON.stringify(payload),
    });
    return response.data;
  },
  async update(id: string, payload: SupplierPayload) {
    const response = await apiRequest<SupplierDto>(`/Fornecedor/${id}`, {
      method: "PUT",
      body: JSON.stringify(payload),
    });
    return response.data;
  },
  async remove(id: string) {
    await apiRequest<object>(`/Fornecedor/${id}`, { method: "DELETE" });
  },
};
