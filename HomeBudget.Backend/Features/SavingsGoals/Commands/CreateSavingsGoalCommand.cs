using HomeBudget.Data.Context;
using HomeBudget.Data.Entities;
using MediatR;

namespace HomeBudget.Application.SavingsGoals.Commands;

public record CreateSavingsGoalCommand(
    string Name,
    decimal TargetAmount,
    DateTime? Deadline,
    string Icon,
    string Color,
    int HouseholdId
) : IRequest<int>;

public class CreateSavingsGoalCommandHandler : IRequestHandler<CreateSavingsGoalCommand, int>
{
    private readonly HomeBudgetDbContext _context;

    public CreateSavingsGoalCommandHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<int> Handle(
        CreateSavingsGoalCommand command,
        CancellationToken cancellationToken)
    {
        var goal = new SavingsGoal
        {
            Name = command.Name,
            TargetAmount = command.TargetAmount,
            CurrentAmount = 0,
            Deadline = command.Deadline,
            Icon = command.Icon,
            Color = command.Color,
            HouseholdId = command.HouseholdId,
            CreatedAt = DateTime.UtcNow,
        };

        _context.SavingsGoals.Add(goal);
        await _context.SaveChangesAsync(cancellationToken);
        return goal.Id;
    }
}
