using HomeBudget.Application.Categories;
using HomeBudget.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Application.Categories.Queries;

public record GetAllCategoriesQuery : IRequest<List<CategoryDto>>;

public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryDto>>
{
    private readonly HomeBudgetDbContext _context;

    public GetAllCategoriesQueryHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<List<CategoryDto>> Handle(
        GetAllCategoriesQuery query,
        CancellationToken cancellationToken) =>
        await _context.Categories
            .OrderBy(category => category.Name)
            .Select(category => new CategoryDto(
                category.Id,
                category.Name,
                category.Icon,
                category.Color,
                category.Type,
                category.IsDefault))
            .ToListAsync(cancellationToken);
}
