using HomeBudget.Data.Enums;

namespace HomeBudget.Data.Entities;

public class ExpenseSplit
{
    public int Id { get; set; }
    public int TransactionId { get; set; }
    public int PaidByUserId { get; set; }
    public int OwesToUserId { get; set; }
    public decimal Amount { get; set; }
    public SplitType SplitType { get; set; } = SplitType.Equal;
    public bool IsSettled { get; set; } = false;
    public DateTime? SettledAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Transaction Transaction { get; set; } = null!;
    public User PaidByUser { get; set; } = null!;
    public User OwesToUser { get; set; } = null!;
}
