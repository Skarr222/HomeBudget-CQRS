using HomeBudget.Application.SavingsGoals;
using HomeBudget.Data.Entities;

namespace HomeBudget.Application.SavingsGoals.Maps;

public static class SavingsGoalMappings
{
    public static SavingsGoalDto ToDto(SavingsGoal goal) =>
        new SavingsGoalDto(
            goal.Id,
            goal.Name,
            goal.TargetAmount,
            goal.CurrentAmount,
            goal.TargetAmount > 0 ? (double)(goal.CurrentAmount / goal.TargetAmount * 100) : 0,
            goal.Deadline,
            goal.Icon,
            goal.Color,
            goal.IsCompleted
        );
}
