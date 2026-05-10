using HomeBudget.Application.ShoppingItems;
using HomeBudget.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Application.ShoppingItems.Queries;

public record GetShoppingItemsQuery(int ShoppingListId) : IRequest<List<ShoppingItemDto>>;

public class GetShoppingItemsQueryHandler
    : IRequestHandler<GetShoppingItemsQuery, List<ShoppingItemDto>>
{
    private readonly HomeBudgetDbContext _context;

    public GetShoppingItemsQueryHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<List<ShoppingItemDto>> Handle(
        GetShoppingItemsQuery query,
        CancellationToken cancellationToken
    ) =>
        await _context
            .ShoppingItems.Where(item => item.ShoppingListId == query.ShoppingListId)
            .OrderBy(item => item.IsChecked)
            .ThenBy(item => item.Id)
            .Select(item => new ShoppingItemDto(
                item.Id,
                item.Name,
                item.Quantity,
                item.EstimatedPrice,
                item.IsChecked,
                item.ShoppingListId
            ))
            .ToListAsync(cancellationToken);
}
