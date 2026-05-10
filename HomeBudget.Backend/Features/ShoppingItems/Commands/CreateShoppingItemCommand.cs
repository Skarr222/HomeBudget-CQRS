using HomeBudget.Data.Context;
using HomeBudget.Data.Entities;
using MediatR;

namespace HomeBudget.Application.ShoppingItems.Commands;

public record CreateShoppingItemCommand(
    string Name,
    int Quantity,
    decimal? EstimatedPrice,
    int ShoppingListId
) : IRequest<int>;

public class CreateShoppingItemCommandHandler : IRequestHandler<CreateShoppingItemCommand, int>
{
    private readonly HomeBudgetDbContext _context;

    public CreateShoppingItemCommandHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<int> Handle(
        CreateShoppingItemCommand command,
        CancellationToken cancellationToken
    )
    {
        var item = new ShoppingItem
        {
            Name = command.Name,
            Quantity = command.Quantity,
            EstimatedPrice = command.EstimatedPrice,
            IsChecked = false,
            ShoppingListId = command.ShoppingListId,
        };

        _context.ShoppingItems.Add(item);
        await _context.SaveChangesAsync(cancellationToken);
        return item.Id;
    }
}
