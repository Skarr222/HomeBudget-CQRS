namespace HomeBudget.Data.Entities;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? HouseholdId { get; set; }

    public Household? Household { get; set; }
    public ICollection<TransactionTag> TransactionTags { get; set; } = new List<TransactionTag>();
}
