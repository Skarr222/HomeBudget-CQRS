using HomeBudget.Application.ShoppingLists;
using HomeBudget.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Application.ShoppingLists.Queries;

public record GetAllShoppingListsQuery(int HouseholdId) : IRequest<List<ShoppingListDto>>;

public class GetAllShoppingListsQueryHandler : IRequestHandler<GetAllShoppingListsQuery, List<ShoppingListDto>>
{
    private readonly HomeBudgetDbContext _context;

    public GetAllShoppingListsQueryHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<List<ShoppingListDto>> Handle(
        GetAllShoppingListsQuery query,
        CancellationToken cancellationToken) =>
        await _context.ShoppingLists
            .Include(shoppingList => shoppingList.CreatedBy)
            .Include(shoppingList => shoppingList.Items)
            .Where(shoppingList => shoppingList.HouseholdId == query.HouseholdId)
            .OrderByDescending(shoppingList => shoppingList.CreatedAt)
            .Select(shoppingList => new ShoppingListDto(
                shoppingList.Id,
                shoppingList.Name,
                shoppingList.IsCompleted,
                shoppingList.Items.Count,
                shoppingList.Items.Count(item => item.IsChecked),
                shoppingList.CreatedBy.FirstName + " " + shoppingList.CreatedBy.LastName,
                shoppingList.CreatedAt))
            .ToListAsync(cancellationToken);
}
