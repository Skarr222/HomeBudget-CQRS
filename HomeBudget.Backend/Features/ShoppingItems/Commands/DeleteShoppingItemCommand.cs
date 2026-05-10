using HomeBudget.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Application.ShoppingItems.Commands;

public record DeleteShoppingItemCommand(int Id) : IRequest<bool>;

public class DeleteShoppingItemCommandHandler : IRequestHandler<DeleteShoppingItemCommand, bool>
{
    private readonly HomeBudgetDbContext _context;

    public DeleteShoppingItemCommandHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<bool> Handle(
        DeleteShoppingItemCommand command,
        CancellationToken cancellationToken)
    {
        var item = await _context.ShoppingItems.FirstOrDefaultAsync(
            item => item.Id == command.Id,
            cancellationToken);

        if (item is null)
            return false;

        _context.ShoppingItems.Remove(item);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
