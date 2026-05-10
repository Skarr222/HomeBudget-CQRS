using HomeBudget.Application.BillPayments;
using HomeBudget.Application.Bills;
using HomeBudget.Data.Context;
using HomeBudget.Data.Enums;
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
        CancellationToken cancellationToken)
    {
        var bills = await _context.Bills
            .Include(bill => bill.Category)
            .Include(bill => bill.Payments)
            .Where(bill => bill.HouseholdId == query.HouseholdId)
            .OrderBy(bill => bill.DueDay)
            .ToListAsync(cancellationToken);

        return bills.Select(bill =>
        {
            var nextPayment = bill.Payments
                .Where(payment => payment.Status != BillStatus.Paid)
                .OrderBy(payment => payment.DueDate)
                .FirstOrDefault();

            var nextPaymentDto = nextPayment is null ? null : new BillPaymentDto(
                nextPayment.Id,
                nextPayment.BillId,
                bill.Name,
                nextPayment.Amount,
                nextPayment.DueDate,
                nextPayment.PaidDate,
                nextPayment.PaymentMethod,
                nextPayment.Status,
                nextPayment.TransactionId);

            return new BillDto(
                bill.Id,
                bill.Name,
                bill.Provider,
                bill.DueDay,
                bill.EstimatedAmount,
                bill.Icon,
                bill.Color,
                bill.IsActive,
                bill.Category.Name,
                bill.CategoryId,
                nextPaymentDto,
                bill.Payments.Where(payment => payment.Status == BillStatus.Paid).Sum(payment => payment.Amount),
                bill.Payments.Count(payment => payment.Status == BillStatus.Paid));
        }).ToList();
    }
}
