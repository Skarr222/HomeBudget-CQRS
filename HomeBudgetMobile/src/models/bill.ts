import { PaymentMethod, BillStatus } from "./enums";

export interface BillPaymentDto {
  id: number;
  billId: number;
  billName: string;
  amount: number;
  dueDate: string;
  paidDate: string | null;
  paymentMethod: PaymentMethod | null;
  status: BillStatus;
  transactionId: number | null;
}

export interface BillDto {
  id: number;
  name: string;
  provider: string;
  dueDay: number;
  estimatedAmount: number;
  icon: string;
  color: string;
  isActive: boolean;
  categoryName: string;
  categoryId: number;
  nextPayment: BillPaymentDto | null;
  totalPaid: number;
  paymentsCount: number;
  paidThisMonth: boolean;
}

export interface CreateBillDto {
  name: string;
  provider: string;
  dueDay: number;
  estimatedAmount: number;
  icon: string;
  color: string;
  householdId: number;
  categoryId: number;
}

export interface UpdateBillDto {
  id: number;
  name: string;
  provider: string;
  dueDay: number;
  estimatedAmount: number;
  icon: string;
  color: string;
  isActive: boolean;
}

export interface PayBillDto {
  billId: number;
  amount: number;
  paymentMethod: string;
  accountId: number;
  userId: number;
  paidDate: string;
}
