using HomeBudget.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Application.SavingsGoals.Commands;

public record UpdateSavingsGoalCommand(
    int Id,
    string Name,
    decimal TargetAmount,
    decimal CurrentAmount,
    DateTime? Deadline,
    string Icon,
    string Color,
    bool IsCompleted
) : IRequest<bool>;

public class UpdateSavingsGoalCommandHandler : IRequestHandler<UpdateSavingsGoalCommand, bool>
{
    private readonly HomeBudgetDbContext _context;

    public UpdateSavingsGoalCommandHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<bool> Handle(
        UpdateSavingsGoalCommand command,
        CancellationToken cancellationToken
    )
    {
        var goal = await _context.SavingsGoals.FirstOrDefaultAsync(
            goal => goal.Id == command.Id,
            cancellationToken
        );

        if (goal is null)
            return false;

        goal.Name = command.Name;
        goal.TargetAmount = command.TargetAmount;
        goal.CurrentAmount = command.CurrentAmount;
        goal.Deadline = command.Deadline;
        goal.Icon = command.Icon;
        goal.Color = command.Color;
        goal.IsCompleted = command.IsCompleted;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
