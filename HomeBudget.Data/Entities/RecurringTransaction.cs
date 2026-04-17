using HomeBudget.Data.Enums;

namespace HomeBudget.Data.Entities;

public class RecurringTransaction
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public Frequency Frequency { get; set; } = Frequency.Monthly;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime NextOccurrence { get; set; }
    public bool IsActive { get; set; } = true;

    public int UserId { get; set; }
    public int CategoryId { get; set; }
    public int? HouseholdId { get; set; }

    public User User { get; set; } = null!;
    public Category Category { get; set; } = null!;
    public Household? Household { get; set; }
    public ICollection<Transaction> GeneratedTransactions { get; set; } = new List<Transaction>();
}
