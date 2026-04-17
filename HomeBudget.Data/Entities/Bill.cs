namespace HomeBudget.Data.Entities;

public class Bill
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public int DueDay { get; set; }
    public decimal EstimatedAmount { get; set; }
    public string Icon { get; set; } = "receipt";
    public string Color { get; set; } = "#F59E0B";
    public bool IsActive { get; set; } = true;

    public int HouseholdId { get; set; }
    public int CategoryId { get; set; }

    public Household Household { get; set; } = null!;
    public Category Category { get; set; } = null!;
    public ICollection<BillPayment> Payments { get; set; } = new List<BillPayment>();
}
