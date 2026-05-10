using HomeBudget.Data.Enums;

namespace HomeBudget.Application.Categories;

public record CategoryDto(
    int Id,
    string Name,
    string Icon,
    string Color,
    CategoryType Type,
    bool IsDefault
);
