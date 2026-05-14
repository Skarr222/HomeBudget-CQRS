using HomeBudget.Application.Budgets;
using HomeBudget.Application.Budgets.Maps;
using HomeBudget.Data.Context;
using HomeBudget.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Application.Budgets.Queries;

public record GetAllBudgetsQuery(int HouseholdId, int? Month, int? Year)
    : IRequest<List<BudgetDto>>;

public class GetAllBudgetsQueryHandler : IRequestHandler<GetAllBudgetsQuery, List<BudgetDto>>
{
    private readonly HomeBudgetDbContext _context;

    public GetAllBudgetsQueryHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<List<BudgetDto>> Handle(
        GetAllBudgetsQuery query,
        CancellationToken cancellationToken
    )
    {
        var month = query.Month ?? DateTime.UtcNow.Month;
        var year = query.Year ?? DateTime.UtcNow.Year;

        var budgets = await _context
            .Budgets.Include(budget => budget.User)
            .Include(budget => budget.Category)
            .Where(budget =>
                budget.HouseholdId == query.HouseholdId
                && budget.Month == month
                && budget.Year == year
            )
            .ToListAsync(cancellationToken);

        var categoryIds = budgets.Select(budget => budget.CategoryId).Distinct().ToList();

        var spentPerCategory = await _context
            .Transactions.Where(transaction =>
                transaction.HouseholdId == query.HouseholdId
                && transaction.Date.Month == month
                && transaction.Date.Year == year
                && transaction.Type == TransactionType.Expense
                && categoryIds.Contains(transaction.CategoryId)
            )
            .GroupBy(transaction => transaction.CategoryId)
            .Select(group => new
            {
                CategoryId = group.Key,
                TotalSpent = group.Sum(transaction => transaction.Amount),
            })
            .ToDictionaryAsync(
                entry => entry.CategoryId,
                entry => entry.TotalSpent,
                cancellationToken
            );

        return budgets
            .Select(budget =>
            {
                var spent = spentPerCategory.GetValueOrDefault(budget.CategoryId, 0);
                return BudgetMappings.ToDto(budget, spent);
            })
            .ToList();
    }
}
