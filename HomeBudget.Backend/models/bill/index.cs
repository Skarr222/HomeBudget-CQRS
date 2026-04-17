using HomeBudget.Application.BillPayments;

namespace HomeBudget.Application.Bills;

public record BillDto(
    int Id,
    string Name,
    string Provider,
    int DueDay,
    decimal EstimatedAmount,
    string Icon,
    string Color,
    bool IsActive,
    string CategoryName,
    int CategoryId,
    BillPaymentDto? NextPayment,
    decimal TotalPaid,
    int PaymentsCount
);

public record CreateBillDto(
    string Name,
    string Provider,
    int DueDay,
    decimal EstimatedAmount,
    string Icon,
    string Color,
    int HouseholdId,
    int CategoryId
);

public record UpdateBillDto(
    string Name,
    string Provider,
    int DueDay,
    decimal EstimatedAmount,
    string Icon,
    string Color,
    bool IsActive
);
