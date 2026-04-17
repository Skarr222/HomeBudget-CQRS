namespace HomeBudget.Application.Budgets;

public record BudgetDto(
    int Id,
    decimal Amount,
    decimal Spent,
    decimal Remaining,
    double Percentage,
    int Month,
    int Year,
    string CategoryName,
    string CategoryIcon,
    string CategoryColor,
    int CategoryId,
    string UserName,
    int UserId
);

public record CreateBudgetDto(
    decimal Amount,
    int Month,
    int Year,
    int UserId,
    int CategoryId,
    int? HouseholdId
);

public record UpdateBudgetDto(decimal Amount);
