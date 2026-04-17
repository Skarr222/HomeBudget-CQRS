using HomeBudget.Data.Entities;
using HomeBudget.Data.Seed;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Data.Context;

public class HomeBudgetDbContext : DbContext
{
    public HomeBudgetDbContext(DbContextOptions<HomeBudgetDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Household> Households => Set<Household>();
    public DbSet<HouseholdMember> HouseholdMembers => Set<HouseholdMember>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<TransactionTag> TransactionTags => Set<TransactionTag>();
    public DbSet<Budget> Budgets => Set<Budget>();
    public DbSet<SavingsGoal> SavingsGoals => Set<SavingsGoal>();
    public DbSet<RecurringTransaction> RecurringTransactions => Set<RecurringTransaction>();
    public DbSet<Bill> Bills => Set<Bill>();
    public DbSet<BillPayment> BillPayments => Set<BillPayment>();
    public DbSet<ShoppingList> ShoppingLists => Set<ShoppingList>();
    public DbSet<ShoppingItem> ShoppingItems => Set<ShoppingItem>();
    public DbSet<ShoppingListMember> ShoppingListMembers => Set<ShoppingListMember>();
    public DbSet<Receipt> Receipts => Set<Receipt>();
    public DbSet<ExpenseSplit> ExpenseSplits => Set<ExpenseSplit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ===== Decimal precisions =====
        modelBuilder.Entity<Transaction>().Property(t => t.Amount).HasPrecision(18, 2);
        modelBuilder.Entity<Budget>().Property(b => b.Amount).HasPrecision(18, 2);
        modelBuilder.Entity<SavingsGoal>().Property(s => s.TargetAmount).HasPrecision(18, 2);
        modelBuilder.Entity<SavingsGoal>().Property(s => s.CurrentAmount).HasPrecision(18, 2);
        modelBuilder.Entity<RecurringTransaction>().Property(r => r.Amount).HasPrecision(18, 2);
        modelBuilder.Entity<Account>().Property(a => a.Balance).HasPrecision(18, 2);
        modelBuilder.Entity<Bill>().Property(b => b.EstimatedAmount).HasPrecision(18, 2);
        modelBuilder.Entity<BillPayment>().Property(bp => bp.Amount).HasPrecision(18, 2);
        modelBuilder.Entity<ShoppingItem>().Property(si => si.EstimatedPrice).HasPrecision(18, 2);
        modelBuilder.Entity<ExpenseSplit>().Property(es => es.Amount).HasPrecision(18, 2);

        // ===== Many-to-many: TransactionTag =====
        modelBuilder.Entity<TransactionTag>().HasKey(tt => new { tt.TransactionId, tt.TagId });
        modelBuilder
            .Entity<TransactionTag>()
            .HasOne(tt => tt.Transaction)
            .WithMany(t => t.TransactionTags)
            .HasForeignKey(tt => tt.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder
            .Entity<TransactionTag>()
            .HasOne(tt => tt.Tag)
            .WithMany(t => t.TransactionTags)
            .HasForeignKey(tt => tt.TagId)
            .OnDelete(DeleteBehavior.Cascade);

        // ===== Many-to-many: HouseholdMember =====
        modelBuilder
            .Entity<HouseholdMember>()
            .HasOne(hm => hm.Household)
            .WithMany(h => h.Members)
            .HasForeignKey(hm => hm.HouseholdId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder
            .Entity<HouseholdMember>()
            .HasOne(hm => hm.User)
            .WithMany(u => u.HouseholdMembers)
            .HasForeignKey(hm => hm.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // ===== Many-to-many: ShoppingListMember =====
        modelBuilder
            .Entity<ShoppingListMember>()
            .HasKey(slm => new { slm.ShoppingListId, slm.UserId });
        modelBuilder
            .Entity<ShoppingListMember>()
            .HasOne(slm => slm.ShoppingList)
            .WithMany(sl => sl.Members)
            .HasForeignKey(slm => slm.ShoppingListId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder
            .Entity<ShoppingListMember>()
            .HasOne(slm => slm.User)
            .WithMany(u => u.ShoppingListMembers)
            .HasForeignKey(slm => slm.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        // ===== Transaction =====
        modelBuilder
            .Entity<Transaction>()
            .HasOne(t => t.User)
            .WithMany(u => u.Transactions)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder
            .Entity<Transaction>()
            .HasOne(t => t.Category)
            .WithMany(c => c.Transactions)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder
            .Entity<Transaction>()
            .HasOne(t => t.Account)
            .WithMany(a => a.Transactions)
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder
            .Entity<Transaction>()
            .HasOne(t => t.RecurringTransaction)
            .WithMany(rt => rt.GeneratedTransactions)
            .HasForeignKey(t => t.RecurringTransactionId)
            .OnDelete(DeleteBehavior.SetNull);

        // ===== Budget =====
        modelBuilder
            .Entity<Budget>()
            .HasOne(b => b.User)
            .WithMany(u => u.Budgets)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder
            .Entity<Budget>()
            .HasOne(b => b.Category)
            .WithMany(c => c.Budgets)
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // ===== RecurringTransaction =====
        modelBuilder
            .Entity<RecurringTransaction>()
            .HasOne(rt => rt.User)
            .WithMany(u => u.RecurringTransactions)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder
            .Entity<RecurringTransaction>()
            .HasOne(rt => rt.Category)
            .WithMany()
            .HasForeignKey(rt => rt.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // ===== Account =====
        modelBuilder
            .Entity<Account>()
            .HasOne(a => a.User)
            .WithMany(u => u.Accounts)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // ===== Category =====
        modelBuilder
            .Entity<Category>()
            .HasOne(c => c.Household)
            .WithMany(h => h.Categories)
            .HasForeignKey(c => c.HouseholdId)
            .OnDelete(DeleteBehavior.SetNull);

        // ===== Bill =====
        modelBuilder
            .Entity<Bill>()
            .HasOne(b => b.Household)
            .WithMany(h => h.Bills)
            .HasForeignKey(b => b.HouseholdId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder
            .Entity<Bill>()
            .HasOne(b => b.Category)
            .WithMany(c => c.Bills)
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // ===== BillPayment =====
        modelBuilder
            .Entity<BillPayment>()
            .HasOne(bp => bp.Bill)
            .WithMany(b => b.Payments)
            .HasForeignKey(bp => bp.BillId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder
            .Entity<BillPayment>()
            .HasOne(bp => bp.Transaction)
            .WithMany()
            .HasForeignKey(bp => bp.TransactionId)
            .OnDelete(DeleteBehavior.SetNull);

        // ===== ShoppingList =====
        modelBuilder
            .Entity<ShoppingList>()
            .HasOne(sl => sl.CreatedBy)
            .WithMany(u => u.CreatedShoppingLists)
            .HasForeignKey(sl => sl.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder
            .Entity<ShoppingList>()
            .HasOne(sl => sl.Household)
            .WithMany(h => h.ShoppingLists)
            .HasForeignKey(sl => sl.HouseholdId)
            .OnDelete(DeleteBehavior.Cascade);

        // ===== ShoppingItem =====
        modelBuilder
            .Entity<ShoppingItem>()
            .HasOne(si => si.ShoppingList)
            .WithMany(sl => sl.Items)
            .HasForeignKey(si => si.ShoppingListId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder
            .Entity<ShoppingItem>()
            .HasOne(si => si.Category)
            .WithMany()
            .HasForeignKey(si => si.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        // ===== Receipt =====
        modelBuilder
            .Entity<Receipt>()
            .HasOne(r => r.Transaction)
            .WithOne(t => t.Receipt)
            .HasForeignKey<Receipt>(r => r.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);

        // ===== ExpenseSplit =====
        modelBuilder
            .Entity<ExpenseSplit>()
            .HasOne(es => es.Transaction)
            .WithMany(t => t.Splits)
            .HasForeignKey(es => es.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder
            .Entity<ExpenseSplit>()
            .HasOne(es => es.PaidByUser)
            .WithMany()
            .HasForeignKey(es => es.PaidByUserId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder
            .Entity<ExpenseSplit>()
            .HasOne(es => es.OwesToUser)
            .WithMany()
            .HasForeignKey(es => es.OwesToUserId)
            .OnDelete(DeleteBehavior.Restrict);

        SeedData.Seed(modelBuilder);
    }
}
