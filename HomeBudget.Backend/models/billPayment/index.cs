using HomeBudget.Data.Enums;

namespace HomeBudget.Application.BillPayments;

public record BillPaymentDto(
    int Id,
    int BillId,
    string BillName,
    decimal Amount,
    DateTime DueDate,
    DateTime? PaidDate,
    PaymentMethod? PaymentMethod,
    BillStatus Status,
    int? TransactionId
);

public record CreateBillPaymentDto(int BillId, decimal Amount, DateTime DueDate);

public record UpdateBillPaymentDto(
    decimal Amount,
    DateTime DueDate,
    DateTime? PaidDate,
    PaymentMethod? PaymentMethod,
    BillStatus Status
);
