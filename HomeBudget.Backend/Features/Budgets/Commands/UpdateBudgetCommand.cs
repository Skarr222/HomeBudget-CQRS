using HomeBudget.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Application.Budgets.Commands;

public record UpdateBudgetCommand(int Id, decimal Amount) : IRequest<bool>;

public class UpdateBudgetCommandHandler : IRequestHandler<UpdateBudgetCommand, bool>
{
    private readonly HomeBudgetDbContext _context;

    public UpdateBudgetCommandHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<bool> Handle(UpdateBudgetCommand command, CancellationToken cancellationToken)
    {
        var budget = await _context.Budgets.FirstOrDefaultAsync(
            budget => budget.Id == command.Id,
            cancellationToken
        );

        if (budget is null)
            return false;

        budget.Amount = command.Amount;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
