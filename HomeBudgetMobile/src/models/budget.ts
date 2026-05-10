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

export interface CreateBudgetDto {
  amount: number;
  month: number;
  year: number;
  userId: number;
  categoryId: number;
  householdId: number;
}
