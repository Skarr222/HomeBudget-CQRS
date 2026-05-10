using HomeBudget.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Application.Budgets.Commands;

public record DeleteBudgetCommand(int Id) : IRequest<bool>;

public class DeleteBudgetCommandHandler : IRequestHandler<DeleteBudgetCommand, bool>
{
    private readonly HomeBudgetDbContext _context;

    public DeleteBudgetCommandHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<bool> Handle(
        DeleteBudgetCommand command,
        CancellationToken cancellationToken)
    {
        var budget = await _context.Budgets
            .FirstOrDefaultAsync(budget => budget.Id == command.Id, cancellationToken);

        if (budget is null) return false;

        _context.Budgets.Remove(budget);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
