import { CategoryType } from "./enums";

export interface CategoryDto {
  id: number;
  name: string;
  icon: string;
  color: string;
  type: CategoryType;
  isDefault: boolean;
}

export interface CategorySpendingDto {
  name: string;
  color: string;
  icon: string;
  amount: number;
  percentage: number;
  transactionCount: number;
}
