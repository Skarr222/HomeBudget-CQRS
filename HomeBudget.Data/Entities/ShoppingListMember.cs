namespace HomeBudget.Data.Entities;

public class ShoppingListMember
{
    public int ShoppingListId { get; set; }
    public int UserId { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    public ShoppingList ShoppingList { get; set; } = null!;
    public User User { get; set; } = null!;
}
