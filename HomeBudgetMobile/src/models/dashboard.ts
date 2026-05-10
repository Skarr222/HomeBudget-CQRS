import { CategorySpendingDto } from "./category";
import { TransactionDto } from "./transaction";
import { BudgetDto } from "./budget";
import { SavingsGoalDto } from "./savingsGoal";
import { BillPaymentDto } from "./bill";
import { AccountDto } from "./account";

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
