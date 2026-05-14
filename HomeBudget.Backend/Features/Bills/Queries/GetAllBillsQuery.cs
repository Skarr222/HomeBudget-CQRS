using HomeBudget.Application.Bills;
using HomeBudget.Application.Bills.Maps;
using HomeBudget.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Application.Bills.Queries;

public record GetAllBillsQuery(int HouseholdId) : IRequest<List<BillDto>>;

public class GetAllBillsQueryHandler : IRequestHandler<GetAllBillsQuery, List<BillDto>>
{
    private readonly HomeBudgetDbContext _context;

    public GetAllBillsQueryHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<List<BillDto>> Handle(
        GetAllBillsQuery query,
        CancellationToken cancellationToken
    )
    {
        var bills = await _context
            .Bills.Include(bill => bill.Category)
            .Include(bill => bill.Payments)
            .Where(bill => bill.HouseholdId == query.HouseholdId)
            .OrderBy(bill => bill.DueDay)
            .ToListAsync(cancellationToken);

        var now = DateTime.UtcNow;

        return bills.Select(bill => BillMappings.ToDto(bill, now)).ToList();
    }
}
