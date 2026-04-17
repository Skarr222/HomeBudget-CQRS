using HomeBudget.Data.Enums;

namespace HomeBudget.Data.Entities;

public class BillPayment
{
    public int Id { get; set; }
    public int BillId { get; set; }
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public BillStatus Status { get; set; } = BillStatus.Pending;
    public int? TransactionId { get; set; }

    public Bill Bill { get; set; } = null!;
    public Transaction? Transaction { get; set; }
}
