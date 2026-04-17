using HomeBudget.Application.Transactions;
using HomeBudget.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Application.Transactions.Queries;

public record GetTransactionByIdQuery(int Id) : IRequest<TransactionDto?>;

public class GetTransactionByIdQueryHandler
    : IRequestHandler<GetTransactionByIdQuery, TransactionDto?>
{
    private readonly HomeBudgetDbContext _db;

    public GetTransactionByIdQueryHandler(HomeBudgetDbContext db) => _db = db;

    public async Task<TransactionDto?> Handle(GetTransactionByIdQuery r, CancellationToken ct) =>
        await _db
            .Transactions.Include(t => t.User)
            .Include(t => t.Category)
            .Include(t => t.Account)
            .Include(t => t.Receipt)
            .Include(t => t.Splits)
            .Include(t => t.TransactionTags)
            .ThenInclude(tt => tt.Tag)
            .Where(t => t.Id == r.Id)
            .Select(t => new TransactionDto(
                t.Id,
                t.Title,
                t.Amount,
                t.Date,
                t.Note,
                t.Type,
                t.PaymentMethod,
                t.IsShared,
                t.User.FirstName + " " + t.User.LastName,
                t.UserId,
                t.Category.Name,
                t.Category.Icon,
                t.Category.Color,
                t.CategoryId,
                t.Account.Name,
                t.AccountId,
                t.TransactionTags.Select(tt => tt.Tag.Name).ToList(),
                t.Receipt != null,
                t.Splits.Any(),
                t.CreatedAt
            ))
            .FirstOrDefaultAsync(ct);
}
