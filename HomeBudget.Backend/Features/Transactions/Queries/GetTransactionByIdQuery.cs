using HomeBudget.Application.Transactions;
using HomeBudget.Application.Transactions.Maps;
using HomeBudget.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Application.Transactions.Queries;

public record GetTransactionByIdQuery(int Id) : IRequest<TransactionDto?>;

public class GetTransactionByIdQueryHandler
    : IRequestHandler<GetTransactionByIdQuery, TransactionDto?>
{
    private readonly HomeBudgetDbContext _context;

    public GetTransactionByIdQueryHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<TransactionDto?> Handle(
        GetTransactionByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        var transaction = await _context
            .Transactions.Include(transaction => transaction.User)
            .Include(transaction => transaction.Category)
            .Include(transaction => transaction.Account)
            .Include(transaction => transaction.Receipt)
            .Include(transaction => transaction.Splits)
            .Include(transaction => transaction.TransactionTags)
            .ThenInclude(transactionTag => transactionTag.Tag)
            .Where(transaction => transaction.Id == query.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return transaction is null ? null : TransactionMappings.ToDto(transaction);
    }
}
