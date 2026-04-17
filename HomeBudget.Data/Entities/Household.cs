namespace HomeBudget.Data.Entities;

public class Household
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string InviteCode { get; set; } = Guid.NewGuid().ToString()[..8].ToUpper();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<HouseholdMember> Members { get; set; } = new List<HouseholdMember>();
    public ICollection<SavingsGoal> SavingsGoals { get; set; } = new List<SavingsGoal>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<Bill> Bills { get; set; } = new List<Bill>();
    public ICollection<ShoppingList> ShoppingLists { get; set; } = new List<ShoppingList>();
}
