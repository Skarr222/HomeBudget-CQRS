using HomeBudget.Data.Context;
using HomeBudget.Data.Entities;
using MediatR;

namespace HomeBudget.Application.Budgets.Commands;

public record CreateBudgetCommand(
    decimal Amount,
    int Month,
    int Year,
    int UserId,
    int CategoryId,
    int HouseholdId
) : IRequest<int>;

public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, int>
{
    private readonly HomeBudgetDbContext _context;

    public CreateBudgetCommandHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<int> Handle(CreateBudgetCommand command, CancellationToken cancellationToken)
    {
        var budget = new Budget
        {
            Amount = command.Amount,
            Month = command.Month,
            Year = command.Year,
            UserId = command.UserId,
            CategoryId = command.CategoryId,
            HouseholdId = command.HouseholdId,
        };

        _context.Budgets.Add(budget);
        await _context.SaveChangesAsync(cancellationToken);
        return budget.Id;
    }
}
