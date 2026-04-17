using HomeBudget.Data.Enums;

namespace HomeBudget.Data.Entities;

public class Transaction
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Note { get; set; }
    public TransactionType Type { get; set; }
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Card;
    public bool IsShared { get; set; } = false;

    public int UserId { get; set; }
    public int CategoryId { get; set; }
    public int AccountId { get; set; }
    public int? HouseholdId { get; set; }
    public int? RecurringTransactionId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public Category Category { get; set; } = null!;
    public Account Account { get; set; } = null!;
    public Household? Household { get; set; }
    public RecurringTransaction? RecurringTransaction { get; set; }
    public Receipt? Receipt { get; set; }
    public ICollection<TransactionTag> TransactionTags { get; set; } = new List<TransactionTag>();
    public ICollection<ExpenseSplit> Splits { get; set; } = new List<ExpenseSplit>();
}
