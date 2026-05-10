using HomeBudget.Application.SavingsGoals;
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
    ) =>
        await _context
            .SavingsGoals.Where(goal => goal.HouseholdId == query.HouseholdId)
            .OrderByDescending(goal => goal.CreatedAt)
            .Select(goal => new SavingsGoalDto(
                goal.Id,
                goal.Name,
                goal.TargetAmount,
                goal.CurrentAmount,
                goal.TargetAmount > 0 ? (double)(goal.CurrentAmount / goal.TargetAmount * 100) : 0,
                goal.Deadline,
                goal.Icon,
                goal.Color,
                goal.IsCompleted
            ))
            .ToListAsync(cancellationToken);
}
