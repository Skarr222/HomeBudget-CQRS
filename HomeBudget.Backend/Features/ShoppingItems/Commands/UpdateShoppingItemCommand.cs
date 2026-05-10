using HomeBudget.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Application.ShoppingItems.Commands;

public record UpdateShoppingItemCommand(
    int Id,
    string Name,
    int Quantity,
    decimal? EstimatedPrice,
    bool IsChecked
) : IRequest<bool>;

public class UpdateShoppingItemCommandHandler : IRequestHandler<UpdateShoppingItemCommand, bool>
{
    private readonly HomeBudgetDbContext _context;

    public UpdateShoppingItemCommandHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<bool> Handle(
        UpdateShoppingItemCommand command,
        CancellationToken cancellationToken)
    {
        var item = await _context.ShoppingItems.FirstOrDefaultAsync(
            item => item.Id == command.Id,
            cancellationToken);

        if (item is null)
            return false;

        item.Name = command.Name;
        item.Quantity = command.Quantity;
        item.EstimatedPrice = command.EstimatedPrice;
        item.IsChecked = command.IsChecked;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
