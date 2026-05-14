using HomeBudget.Application.Transactions;
using HomeBudget.Application.Transactions.Maps;
using HomeBudget.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Application.Transactions.Queries;

public record GetAllTransactionsQuery(
    int? HouseholdId,
    int? UserId,
    int? CategoryId,
    int? AccountId,
    int? Month,
    int? Year
) : IRequest<List<TransactionDto>>;

public class GetAllTransactionsQueryHandler
    : IRequestHandler<GetAllTransactionsQuery, List<TransactionDto>>
{
    private readonly HomeBudgetDbContext _context;

    public GetAllTransactionsQueryHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<List<TransactionDto>> Handle(
        GetAllTransactionsQuery query,
        CancellationToken cancellationToken
    )
    {
        var transactions = _context
            .Transactions.Include(transaction => transaction.User)
            .Include(transaction => transaction.Category)
            .Include(transaction => transaction.Account)
            .Include(transaction => transaction.Receipt)
            .Include(transaction => transaction.Splits)
            .Include(transaction => transaction.TransactionTags)
            .ThenInclude(transactionTag => transactionTag.Tag)
            .AsQueryable();

        if (query.HouseholdId.HasValue)
        {
            transactions = transactions.Where(transaction =>
                transaction.HouseholdId == query.HouseholdId
            );
        }
        if (query.UserId.HasValue)
        {
            transactions = transactions.Where(transaction => transaction.UserId == query.UserId);
        }
        if (query.CategoryId.HasValue)
        {
            transactions = transactions.Where(transaction =>
                transaction.CategoryId == query.CategoryId
            );
        }
        if (query.AccountId.HasValue)
        {
            transactions = transactions.Where(transaction =>
                transaction.AccountId == query.AccountId
            );
        }
        if (query.Month.HasValue && query.Year.HasValue)
        {
            transactions = transactions.Where(transaction =>
                transaction.Date.Month == query.Month && transaction.Date.Year == query.Year
            );
        }

        var result = await transactions
            .OrderByDescending(transaction => transaction.Date)
            .ToListAsync(cancellationToken);

        return result.Select(TransactionMappings.ToDto).ToList();
    }
}
