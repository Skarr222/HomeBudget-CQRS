using HomeBudget.Application.Accounts;
using HomeBudget.Application.BillPayments;
using HomeBudget.Application.Budgets;
using HomeBudget.Application.SavingsGoals;
using HomeBudget.Application.Transactions;

namespace HomeBudget.Application.Dashboard;

public record DashboardDto(
    decimal TotalIncome,
    decimal TotalExpenses,
    decimal Balance,
    List<CategorySpendingDto> TopCategories,
    List<TransactionDto> RecentTransactions,
    List<BudgetDto> ActiveBudgets,
    List<SavingsGoalDto> SavingsGoals,
    List<BillPaymentDto> UpcomingBills,
    List<AccountDto> Accounts
);

public record CategorySpendingDto(
    string Name,
    string Color,
    string Icon,
    decimal Amount,
    double Percentage,
    int TransactionCount
);

public record StatisticsDto(
    decimal TotalIncome,
    decimal TotalExpenses,
    decimal Balance,
    List<CategorySpendingDto> ExpensesByCategory,
    List<CategorySpendingDto> IncomeByCategory,
    List<MonthlyStatDto> MonthlyTrend,
    List<DailyStatDto> DailyTrend,
    Dictionary<string, decimal> SpendingByPaymentMethod
);

public record MonthlyStatDto(
    int Year,
    int Month,
    string MonthName,
    decimal Income,
    decimal Expenses
);

public record DailyStatDto(DateTime Date, decimal Amount);
