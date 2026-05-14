using HomeBudget.Application.Accounts;
using HomeBudget.Application.Accounts.Maps;
using HomeBudget.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Application.Accounts.Queries;

public record GetAllAccountsQuery(int? UserId, int? HouseholdId) : IRequest<List<AccountDto>>;

public class GetAllAccountsQueryHandler : IRequestHandler<GetAllAccountsQuery, List<AccountDto>>
{
    private readonly HomeBudgetDbContext _context;

    public GetAllAccountsQueryHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<List<AccountDto>> Handle(
        GetAllAccountsQuery query,
        CancellationToken cancellationToken
    )
    {
        var accounts = _context.Accounts.Include(account => account.User).AsQueryable();

        if (query.UserId.HasValue)
        {
            accounts = accounts.Where(account => account.UserId == query.UserId);
        }

        if (query.HouseholdId.HasValue)
        {
            accounts = accounts.Where(account =>
                _context.HouseholdMembers.Any(member =>
                    member.HouseholdId == query.HouseholdId && member.UserId == account.UserId
                )
            );
        }

        var result = await accounts
            .OrderBy(account => account.Name)
            .ToListAsync(cancellationToken);

        return result.Select(AccountMappings.ToDto).ToList();
    }
}
