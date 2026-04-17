using HomeBudget.Application.Transactions;
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
    private readonly HomeBudgetDbContext _db;

    public GetAllTransactionsQueryHandler(HomeBudgetDbContext db) => _db = db;

    public async Task<List<TransactionDto>> Handle(GetAllTransactionsQuery r, CancellationToken ct)
    {
        var q = _db
            .Transactions.Include(t => t.User)
            .Include(t => t.Category)
            .Include(t => t.Account)
            .Include(t => t.Receipt)
            .Include(t => t.Splits)
            .Include(t => t.TransactionTags)
            .ThenInclude(tt => tt.Tag)
            .AsQueryable();

        if (r.HouseholdId.HasValue)
            q = q.Where(t => t.HouseholdId == r.HouseholdId);
        if (r.UserId.HasValue)
            q = q.Where(t => t.UserId == r.UserId);
        if (r.CategoryId.HasValue)
            q = q.Where(t => t.CategoryId == r.CategoryId);
        if (r.AccountId.HasValue)
            q = q.Where(t => t.AccountId == r.AccountId);
        if (r.Month.HasValue && r.Year.HasValue)
            q = q.Where(t => t.Date.Month == r.Month && t.Date.Year == r.Year);

        return await q.OrderByDescending(t => t.Date)
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
            .ToListAsync(ct);
    }
}
