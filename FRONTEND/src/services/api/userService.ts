import type { AdminUser } from "@/components/Admin/UsersPage";
import { apiRequest } from "./apiClient";

const USUARIOS_API_URL =
  import.meta.env.VITE_USUARIOS_API_URL ?? "http://localhost:5260/api/Usuario";

export const userService = {
  async list() {
    const response = await apiRequest<AdminUser[]>(USUARIOS_API_URL);
    return response.data ?? [];
  },
};
