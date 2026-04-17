import axios from "axios";
import { TransactionDto, DashboardDto } from "../models/types";

const BASE_URL = "http://10.0.2.2:5067/api";

const api = axios.create({
  baseURL: BASE_URL,
  headers: { "Content-Type": "application/json" },
});

export const transactionsApi = {
  getAll: (params?: { householdId?: number; month?: number; year?: number }) =>
    api.get<TransactionDto[]>("/transactions", { params }),
  delete: (id: number) => api.delete(`/transactions/${id}`),
};

export const dashboardApi = {
  get: (householdId: number, month?: number, year?: number) =>
    api.get<DashboardDto>(`/dashboard/${householdId}`, {
      params: { month, year },
    }),
};

export default api;
