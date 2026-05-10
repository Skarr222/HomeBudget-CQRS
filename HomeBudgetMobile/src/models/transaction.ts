import { TransactionType, PaymentMethod } from "./enums";

export type TransactionFilter = "all" | "expense" | "income";

export interface TransactionDto {
  id: number;
  title: string;
  amount: number;
  date: string;
  note: string | null;
  type: TransactionType;
  paymentMethod: PaymentMethod;
  isShared: boolean;
  userName: string;
  userId: number;
  categoryName: string;
  categoryIcon: string;
  categoryColor: string;
  categoryId: number;
  accountName: string;
  accountId: number;
  tags: string[];
  hasReceipt: boolean;
  hasSplit: boolean;
  createdAt: string;
}

export interface CreateTransactionDto {
  title: string;
  amount: number;
  date: string;
  note?: string;
  type: TransactionType;
  paymentMethod: PaymentMethod;
  isShared: boolean;
  userId: number;
  categoryId: number;
  accountId: number;
  householdId?: number;
  tagIds?: number[];
}

export interface UpdateTransactionDto {
  id: number;
  title: string;
  amount: number;
  date: string;
  note?: string;
  type: TransactionType;
  paymentMethod: PaymentMethod;
  isShared: boolean;
  categoryId: number;
  accountId: number;
  tagIds?: number[];
}
