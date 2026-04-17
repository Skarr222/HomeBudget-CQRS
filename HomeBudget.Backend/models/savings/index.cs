namespace HomeBudget.Application.SavingsGoals;

public record SavingsGoalDto(
    int Id,
    string Name,
    decimal TargetAmount,
    decimal CurrentAmount,
    double Percentage,
    DateTime? Deadline,
    string Icon,
    string Color,
    bool IsCompleted
);

public record CreateSavingsGoalDto(
    string Name,
    decimal TargetAmount,
    DateTime? Deadline,
    string Icon,
    string Color,
    int HouseholdId
);

public record UpdateSavingsGoalDto(
    string? Name,
    decimal? TargetAmount,
    decimal? CurrentAmount,
    DateTime? Deadline,
    bool? IsCompleted
);
