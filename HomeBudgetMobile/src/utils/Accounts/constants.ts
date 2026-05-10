import { AccountType } from "../../models/types";

export const ACCOUNT_TYPES: { value: AccountType; label: string; icon: string }[] = [
  { value: AccountType.Checking,   label: "Konto ROR",        icon: "account-balance" },
  { value: AccountType.Savings,    label: "Oszczędności",     icon: "savings" },
  { value: AccountType.Cash,       label: "Gotówka",          icon: "payments" },
  { value: AccountType.CreditCard, label: "Karta kredytowa",  icon: "credit-card" },
];

export const PRESET_COLORS = [
  "#6366F1",
  "#0077C8",
  "#10B981",
  "#EF4444",
  "#F59E0B",
  "#3B82F6",
  "#8B5CF6",
  "#E3000F",
];
