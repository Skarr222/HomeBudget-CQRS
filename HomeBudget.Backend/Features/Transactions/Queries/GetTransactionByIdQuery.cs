using HomeBudget.Application.Transactions;
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
    ) =>
        await _context
            .Transactions.Include(transaction => transaction.User)
            .Include(transaction => transaction.Category)
            .Include(transaction => transaction.Account)
            .Include(transaction => transaction.Receipt)
            .Include(transaction => transaction.Splits)
            .Include(transaction => transaction.TransactionTags)
            .ThenInclude(transactionTag => transactionTag.Tag)
            .Where(transaction => transaction.Id == query.Id)
            .Select(transaction => new TransactionDto(
                transaction.Id,
                transaction.Title,
                transaction.Amount,
                transaction.Date,
                transaction.Note,
                transaction.Type,
                transaction.PaymentMethod,
                transaction.IsShared,
                transaction.User.FirstName + " " + transaction.User.LastName,
                transaction.UserId,
                transaction.Category.Name,
                transaction.Category.Icon,
                transaction.Category.Color,
                transaction.CategoryId,
                transaction.Account.Name,
                transaction.AccountId,
                transaction.TransactionTags.Select(tt => tt.Tag.Name).ToList(),
                transaction.Receipt != null,
                transaction.Splits.Any(),
                transaction.CreatedAt
            ))
            .FirstOrDefaultAsync(cancellationToken);
}
