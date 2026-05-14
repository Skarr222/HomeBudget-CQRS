using HomeBudget.Application.BillPayments;
using HomeBudget.Data.Entities;

namespace HomeBudget.Application.Bills.Maps;

public static class BillPaymentMappings
{
    public static BillPaymentDto ToDto(BillPayment payment, string billName) =>
        new BillPaymentDto(
            payment.Id,
            payment.BillId,
            billName,
            payment.Amount,
            payment.DueDate,
            payment.PaidDate,
            payment.PaymentMethod,
            payment.Status,
            payment.TransactionId
        );
}
