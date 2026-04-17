using HomeBudget.Data.Enums;

namespace HomeBudget.Data.Entities;

public class HouseholdMember
{
    public int Id { get; set; }
    public int HouseholdId { get; set; }
    public int UserId { get; set; }
    public HouseholdRole Role { get; set; } = HouseholdRole.Member;
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    public Household Household { get; set; } = null!;
    public User User { get; set; } = null!;
}
