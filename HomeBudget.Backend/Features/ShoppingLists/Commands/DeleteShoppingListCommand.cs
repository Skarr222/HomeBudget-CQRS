using HomeBudget.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Application.ShoppingLists.Commands;

public record DeleteShoppingListCommand(int Id) : IRequest<bool>;

public class DeleteShoppingListCommandHandler : IRequestHandler<DeleteShoppingListCommand, bool>
{
    private readonly HomeBudgetDbContext _context;

    public DeleteShoppingListCommandHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<bool> Handle(
        DeleteShoppingListCommand command,
        CancellationToken cancellationToken)
    {
        var shoppingList = await _context.ShoppingLists
            .FirstOrDefaultAsync(shoppingList => shoppingList.Id == command.Id, cancellationToken);

        if (shoppingList is null) return false;

        _context.ShoppingLists.Remove(shoppingList);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
