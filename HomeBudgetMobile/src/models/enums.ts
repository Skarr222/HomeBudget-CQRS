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
