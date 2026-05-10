using HomeBudget.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Application.ShoppingLists.Commands;

public record UpdateShoppingListCommand(int Id, string Name, bool IsCompleted) : IRequest<bool>;

public class UpdateShoppingListCommandHandler : IRequestHandler<UpdateShoppingListCommand, bool>
{
    private readonly HomeBudgetDbContext _context;

    public UpdateShoppingListCommandHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<bool> Handle(
        UpdateShoppingListCommand command,
        CancellationToken cancellationToken)
    {
        var shoppingList = await _context.ShoppingLists
            .FirstOrDefaultAsync(shoppingList => shoppingList.Id == command.Id, cancellationToken);

        if (shoppingList is null) return false;

        shoppingList.Name        = command.Name;
        shoppingList.IsCompleted = command.IsCompleted;

        if (command.IsCompleted && shoppingList.CompletedAt is null)
            shoppingList.CompletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
