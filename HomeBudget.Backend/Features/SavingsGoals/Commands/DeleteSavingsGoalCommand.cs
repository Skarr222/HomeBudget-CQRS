using HomeBudget.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Application.SavingsGoals.Commands;

public record DeleteSavingsGoalCommand(int Id) : IRequest<bool>;

public class DeleteSavingsGoalCommandHandler : IRequestHandler<DeleteSavingsGoalCommand, bool>
{
    private readonly HomeBudgetDbContext _context;

    public DeleteSavingsGoalCommandHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<bool> Handle(
        DeleteSavingsGoalCommand command,
        CancellationToken cancellationToken
    )
    {
        var goal = await _context.SavingsGoals.FirstOrDefaultAsync(
            goal => goal.Id == command.Id,
            cancellationToken
        );

        if (goal is null)
            return false;

        _context.SavingsGoals.Remove(goal);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
