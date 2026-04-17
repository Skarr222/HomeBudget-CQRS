namespace HomeBudget.Data.Entities;

public class Receipt
{
    public int Id { get; set; }
    public int TransactionId { get; set; }
    public string ImagePath { get; set; } = string.Empty;
    public string? StoreName { get; set; }
    public DateTime? ReceiptDate { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Transaction Transaction { get; set; } = null!;
}
