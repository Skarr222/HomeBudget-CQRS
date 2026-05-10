using HomeBudget.Data.Context;
using HomeBudget.Data.Entities;
using MediatR;

namespace HomeBudget.Application.ShoppingLists.Commands;

public record CreateShoppingListCommand(string Name, int CreatedByUserId, int HouseholdId)
    : IRequest<int>;

public class CreateShoppingListCommandHandler : IRequestHandler<CreateShoppingListCommand, int>
{
    private readonly HomeBudgetDbContext _context;

    public CreateShoppingListCommandHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<int> Handle(
        CreateShoppingListCommand command,
        CancellationToken cancellationToken
    )
    {
        var shoppingList = new ShoppingList
        {
            Name = command.Name,
            CreatedByUserId = command.CreatedByUserId,
            HouseholdId = command.HouseholdId,
            CreatedAt = DateTime.UtcNow,
        };

        _context.ShoppingLists.Add(shoppingList);
        await _context.SaveChangesAsync(cancellationToken);
        return shoppingList.Id;
    }
}
