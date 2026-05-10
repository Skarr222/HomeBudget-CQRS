namespace HomeBudget.Application.ShoppingLists;

public record ShoppingListDto(
    int Id,
    string Name,
    bool IsCompleted,
    int ItemCount,
    int CheckedCount,
    string CreatedByName,
    DateTime CreatedAt
);

public record CreateShoppingListDto(string Name, int CreatedByUserId, int HouseholdId);

public record UpdateShoppingListDto(string Name, bool IsCompleted);
