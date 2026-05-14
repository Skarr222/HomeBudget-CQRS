using HomeBudget.Application.Budgets;
using HomeBudget.Data.Entities;

namespace HomeBudget.Application.Budgets.Maps;

public static class BudgetMappings
{
    public static BudgetDto ToDto(Budget budget, decimal spent) =>
        new BudgetDto(
            budget.Id,
            budget.Amount,
            spent,
            budget.Amount - spent,
            budget.Amount > 0 ? (double)(spent / budget.Amount * 100) : 0,
            budget.Month,
            budget.Year,
            budget.Category.Name,
            budget.Category.Icon,
            budget.Category.Color,
            budget.CategoryId,
            budget.User.FirstName + " " + budget.User.LastName,
            budget.UserId
        );
}
