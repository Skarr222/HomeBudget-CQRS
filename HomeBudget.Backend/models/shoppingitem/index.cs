namespace HomeBudget.Application.ShoppingItems;

public record ShoppingItemDto(
    int Id,
    string Name,
    int Quantity,
    decimal? EstimatedPrice,
    bool IsChecked,
    int ShoppingListId
);

public record CreateShoppingItemDto(string Name, int Quantity, decimal? EstimatedPrice, int ShoppingListId);

public record UpdateShoppingItemDto(int Id, string Name, int Quantity, decimal? EstimatedPrice, bool IsChecked);
