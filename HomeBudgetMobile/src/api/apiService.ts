import axios from "axios";
import {
  AccountDto,
  BillDto,
  BudgetDto,
  CategoryDto,
  CreateAccountDto,
  CreateBillDto,
  CreateBudgetDto,
  CreateSavingsGoalDto,
  CreateTransactionDto,
  DashboardDto,
  SavingsGoalDto,
  ShoppingItemDto,
  ShoppingListDto,
  TransactionDto,
  UpdateAccountDto,
  UpdateBillDto,
  UpdateSavingsGoalDto,
  UpdateTransactionDto,
} from "../models/types";

const BASE_URL = "http://10.0.2.2:5067/api";

const api = axios.create({
  baseURL: BASE_URL,
  headers: { "Content-Type": "application/json" },
});

// ── Transactions ─────────────────────────────────────────────────────────────
export const transactionsApi = {
  getAll: (params?: {
    householdId?: number;
    userId?: number;
    categoryId?: number;
    accountId?: number;
    month?: number;
    year?: number;
  }) => api.get<TransactionDto[]>("/transactions", { params }),

  getById: (id: number) => api.get<TransactionDto>(`/transactions/${id}`),

  create: (dto: CreateTransactionDto) =>
    api.post<number>("/transactions", dto),

  update: (id: number, dto: UpdateTransactionDto) =>
    api.put(`/transactions/${id}`, dto),

  delete: (id: number) => api.delete(`/transactions/${id}`),
};

// ── Categories ───────────────────────────────────────────────────────────────
export const categoriesApi = {
  getAll: () => api.get<CategoryDto[]>("/categories"),
};

// ── Accounts ─────────────────────────────────────────────────────────────────
export const accountsApi = {
  getAll: (params?: { userId?: number; householdId?: number }) =>
    api.get<AccountDto[]>("/accounts", { params }),

  create: (dto: CreateAccountDto) => api.post<number>("/accounts", dto),

  update: (id: number, dto: UpdateAccountDto) =>
    api.put(`/accounts/${id}`, dto),

  delete: (id: number) => api.delete(`/accounts/${id}`),
};

// ── Budgets ──────────────────────────────────────────────────────────────────
export const budgetsApi = {
  getAll: (params: { householdId: number; month?: number; year?: number }) =>
    api.get<BudgetDto[]>("/budgets", { params }),

  create: (dto: CreateBudgetDto) => api.post<number>("/budgets", dto),

  update: (id: number, amount: number) =>
    api.put(`/budgets/${id}`, { id, amount }),

  delete: (id: number) => api.delete(`/budgets/${id}`),
};

// ── SavingsGoals ─────────────────────────────────────────────────────────────
export const savingsGoalsApi = {
  getAll: (householdId: number) =>
    api.get<SavingsGoalDto[]>("/savingsGoals", { params: { householdId } }),

  create: (dto: CreateSavingsGoalDto) =>
    api.post<number>("/savingsGoals", dto),

  update: (id: number, dto: UpdateSavingsGoalDto) =>
    api.put(`/savingsGoals/${id}`, dto),

  delete: (id: number) => api.delete(`/savingsGoals/${id}`),
};

// ── Bills ────────────────────────────────────────────────────────────────────
export const billsApi = {
  getAll: (householdId: number) =>
    api.get<BillDto[]>("/bills", { params: { householdId } }),

  create: (dto: CreateBillDto) => api.post<number>("/bills", dto),

  update: (id: number, dto: UpdateBillDto) => api.put(`/bills/${id}`, dto),

  delete: (id: number) => api.delete(`/bills/${id}`),
};

// ── Shopping Lists ────────────────────────────────────────────────────────────
export const shoppingListsApi = {
  getAll: (householdId: number) =>
    api.get<ShoppingListDto[]>("/shoppingLists", { params: { householdId } }),

  create: (name: string, createdByUserId: number, householdId: number) =>
    api.post<number>("/shoppingLists", { name, createdByUserId, householdId }),

  update: (id: number, name: string, isCompleted: boolean) =>
    api.put(`/shoppingLists/${id}`, { id, name, isCompleted }),

  delete: (id: number) => api.delete(`/shoppingLists/${id}`),
};

// ── Shopping Items ────────────────────────────────────────────────────────────
export const shoppingItemsApi = {
  getAll: (shoppingListId: number) =>
    api.get<ShoppingItemDto[]>("/shoppingItems", { params: { shoppingListId } }),

  create: (dto: { name: string; quantity: number; estimatedPrice?: number | null; shoppingListId: number }) =>
    api.post<number>("/shoppingItems", dto),

  update: (id: number, dto: { id: number; name: string; quantity: number; estimatedPrice?: number | null; isChecked: boolean }) =>
    api.put(`/shoppingItems/${id}`, dto),

  delete: (id: number) => api.delete(`/shoppingItems/${id}`),
};

// ── Dashboard ────────────────────────────────────────────────────────────────
export const dashboardApi = {
  get: (householdId: number, month?: number, year?: number) =>
    api.get<DashboardDto>(`/dashboard/${householdId}`, {
      params: { month, year },
    }),
};

export default api;
