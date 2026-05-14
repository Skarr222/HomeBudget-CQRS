using HomeBudget.Application.Bills;
using HomeBudget.Data.Entities;
using HomeBudget.Data.Enums;

namespace HomeBudget.Application.Bills.Maps;

public static class BillMappings
{
    public static BillDto ToDto(Bill bill, DateTime now)
    {
        var nextPayment = bill
            .Payments.Where(p => p.Status != BillStatus.Paid)
            .OrderBy(p => p.DueDate)
            .FirstOrDefault();

        var nextPaymentDto = nextPayment is null
            ? null
            : BillPaymentMappings.ToDto(nextPayment, bill.Name);

        var paidThisMonth = bill.Payments.Any(p =>
            p.Status == BillStatus.Paid
            && p.PaidDate.HasValue
            && p.PaidDate.Value.Month == now.Month
            && p.PaidDate.Value.Year == now.Year
        );

        var paidPayments = bill.Payments.Where(p => p.Status == BillStatus.Paid).ToList();

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
            paidPayments.Sum(p => p.Amount),
            paidPayments.Count,
            paidThisMonth
        );
    }
}
