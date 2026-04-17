namespace HomeBudget.Data.Entities;

public class Budget
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }

    public int UserId { get; set; }
    public int CategoryId { get; set; }
    public int? HouseholdId { get; set; }

    public User User { get; set; } = null!;
    public Category Category { get; set; } = null!;
    public Household? Household { get; set; }
}
