using HomeBudget.Data.Enums;

namespace HomeBudget.Data.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Color { get; set; } = "#6B7280";
    public CategoryType Type { get; set; } = CategoryType.Expense;
    public bool IsDefault { get; set; } = false;
    public int? HouseholdId { get; set; }

    public Household? Household { get; set; }
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
    public ICollection<Bill> Bills { get; set; } = new List<Bill>();
}
