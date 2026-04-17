namespace HomeBudget.Data.Entities;

public class ShoppingItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;
    public decimal? EstimatedPrice { get; set; }
    public bool IsChecked { get; set; } = false;

    public int ShoppingListId { get; set; }
    public int? CategoryId { get; set; }

    public ShoppingList ShoppingList { get; set; } = null!;
    public Category? Category { get; set; }
}
