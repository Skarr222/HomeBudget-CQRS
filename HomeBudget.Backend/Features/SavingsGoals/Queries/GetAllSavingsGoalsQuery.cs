using HomeBudget.Application.SavingsGoals;
using HomeBudget.Application.SavingsGoals.Maps;
using HomeBudget.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Application.SavingsGoals.Queries;

public record GetAllSavingsGoalsQuery(int HouseholdId) : IRequest<List<SavingsGoalDto>>;

public class GetAllSavingsGoalsQueryHandler
    : IRequestHandler<GetAllSavingsGoalsQuery, List<SavingsGoalDto>>
{
    private readonly HomeBudgetDbContext _context;

    public GetAllSavingsGoalsQueryHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<List<SavingsGoalDto>> Handle(
        GetAllSavingsGoalsQuery query,
        CancellationToken cancellationToken
    )
    {
        var goals = await _context
            .SavingsGoals.Where(goal => goal.HouseholdId == query.HouseholdId)
            .OrderByDescending(goal => goal.CreatedAt)
            .ToListAsync(cancellationToken);

        return goals.Select(SavingsGoalMappings.ToDto).ToList();
    }
}
