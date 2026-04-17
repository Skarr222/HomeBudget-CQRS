// ==== Enums ====
export enum TransactionType {
  Expense = "Expense",
  Income = "Income",
}
export enum PaymentMethod {
  Cash = "Cash",
  Card = "Card",
  Transfer = "Transfer",
  BLIK = "BLIK",
}
export enum CategoryType {
  Expense = "Expense",
  Income = "Income",
  Both = "Both",
}
export enum Frequency {
  Daily = "Daily",
  Weekly = "Weekly",
  Monthly = "Monthly",
  Yearly = "Yearly",
}
export enum AccountType {
  Checking = "Checking",
  Savings = "Savings",
  Cash = "Cash",
  CreditCard = "CreditCard",
}
export enum BillStatus {
  Pending = "Pending",
  Paid = "Paid",
  Overdue = "Overdue",
}
export enum SplitType {
  Equal = "Equal",
  AllPayer = "AllPayer",
  AllOther = "AllOther",
  Custom = "Custom",
}

// ==== DTOs ====
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

export interface CategorySpendingDto {
  name: string;
  color: string;
  icon: string;
  amount: number;
  percentage: number;
  transactionCount: number;
}

export interface BudgetDto {
  id: number;
  amount: number;
  spent: number;
  remaining: number;
  percentage: number;
  month: number;
  year: number;
  categoryName: string;
  categoryIcon: string;
  categoryColor: string;
  categoryId: number;
  userName: string;
  userId: number;
}

export interface SavingsGoalDto {
  id: number;
  name: string;
  targetAmount: number;
  currentAmount: number;
  percentage: number;
  deadline: string | null;
  icon: string;
  color: string;
  isCompleted: boolean;
}

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

export interface AccountDto {
  id: number;
  name: string;
  type: AccountType;
  balance: number;
  color: string;
  icon: string;
  userId: number;
  userName: string;
}

export interface DashboardDto {
  totalIncome: number;
  totalExpenses: number;
  balance: number;
  topCategories: CategorySpendingDto[];
  recentTransactions: TransactionDto[];
  activeBudgets: BudgetDto[];
  savingsGoals: SavingsGoalDto[];
  upcomingBills: BillPaymentDto[];
  accounts: AccountDto[];
}
