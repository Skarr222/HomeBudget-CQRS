namespace HomeBudget.Data.Entities;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Currency { get; set; } = "PLN";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
    public ICollection<RecurringTransaction> RecurringTransactions { get; set; } = new List<RecurringTransaction>();
    public ICollection<HouseholdMember> HouseholdMembers { get; set; } = new List<HouseholdMember>();
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
    public ICollection<ShoppingList> CreatedShoppingLists { get; set; } = new List<ShoppingList>();
    public ICollection<ShoppingListMember> ShoppingListMembers { get; set; } = new List<ShoppingListMember>();
}
