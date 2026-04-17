namespace HomeBudget.Data.Entities;

public class SavingsGoal
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal TargetAmount { get; set; }
    public decimal CurrentAmount { get; set; } = 0;
    public DateTime? Deadline { get; set; }
    public string Icon { get; set; } = "piggy-bank";
    public string Color { get; set; } = "#10B981";
    public bool IsCompleted { get; set; } = false;

    public int HouseholdId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Household Household { get; set; } = null!;
}
