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

export interface CreateSavingsGoalDto {
  name: string;
  targetAmount: number;
  deadline?: string;
  icon: string;
  color: string;
  householdId: number;
}

export interface UpdateSavingsGoalDto {
  id: number;
  name: string;
  targetAmount: number;
  currentAmount: number;
  deadline?: string | null;
  icon: string;
  color: string;
  isCompleted: boolean;
}
