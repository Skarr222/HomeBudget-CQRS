namespace HomeBudget.Data.Entities;

public class ShoppingList
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    public int CreatedByUserId { get; set; }
    public int HouseholdId { get; set; }

    public User CreatedBy { get; set; } = null!;
    public Household Household { get; set; } = null!;
    public ICollection<ShoppingItem> Items { get; set; } = new List<ShoppingItem>();
    public ICollection<ShoppingListMember> Members { get; set; } = new List<ShoppingListMember>();
}
