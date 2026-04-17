using HomeBudget.Data.Entities;
using HomeBudget.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Data.Seed;

public static class SeedData
{
    public static void Seed(ModelBuilder mb)
    {
        // ===== USERS =====
        mb.Entity<User>()
            .HasData(
                new User { Id = 1, FirstName = "Oskar", LastName = "Gomza", Email = "oskar@homebudget.pl", Currency = "PLN", CreatedAt = new DateTime(2025, 1, 1) },
                new User { Id = 2, FirstName = "Anna", LastName = "Kowalska", Email = "anna@homebudget.pl", Currency = "PLN", CreatedAt = new DateTime(2025, 1, 1) }
            );

        // ===== HOUSEHOLD =====
        mb.Entity<Household>()
            .HasData(new Household { Id = 1, Name = "Nasz Dom", InviteCode = "HOME2025", CreatedAt = new DateTime(2025, 1, 1) });

        mb.Entity<HouseholdMember>()
            .HasData(
                new HouseholdMember { Id = 1, HouseholdId = 1, UserId = 1, Role = HouseholdRole.Owner, JoinedAt = new DateTime(2025, 1, 1) },
                new HouseholdMember { Id = 2, HouseholdId = 1, UserId = 2, Role = HouseholdRole.Member, JoinedAt = new DateTime(2025, 1, 2) }
            );

        // ===== ACCOUNTS =====
        mb.Entity<Account>()
            .HasData(
                new Account { Id = 1, Name = "PKO BP", Type = AccountType.Checking, Balance = 4850.30m, Color = "#0077C8", Icon = "account-balance", UserId = 1, CreatedAt = new DateTime(2025, 1, 1) },
                new Account { Id = 2, Name = "Revolut", Type = AccountType.Checking, Balance = 1230.50m, Color = "#0075EB", Icon = "credit-card", UserId = 1, CreatedAt = new DateTime(2025, 1, 1) },
                new Account { Id = 3, Name = "Gotówka", Type = AccountType.Cash, Balance = 320.00m, Color = "#10B981", Icon = "payments", UserId = 1, CreatedAt = new DateTime(2025, 1, 1) },
                new Account { Id = 4, Name = "mBank", Type = AccountType.Checking, Balance = 3420.00m, Color = "#E3000F", Icon = "account-balance", UserId = 2, CreatedAt = new DateTime(2025, 1, 1) },
                new Account { Id = 5, Name = "Oszczędności", Type = AccountType.Savings, Balance = 12000.00m, Color = "#F59E0B", Icon = "savings", UserId = 1, CreatedAt = new DateTime(2025, 1, 1) }
            );

        // ===== CATEGORIES =====
        mb.Entity<Category>()
            .HasData(
                new Category { Id = 1, Name = "Jedzenie", Icon = "restaurant", Color = "#EF4444", Type = CategoryType.Expense, IsDefault = true },
                new Category { Id = 2, Name = "Transport", Icon = "directions-car", Color = "#3B82F6", Type = CategoryType.Expense, IsDefault = true },
                new Category { Id = 3, Name = "Rachunki", Icon = "receipt", Color = "#F59E0B", Type = CategoryType.Expense, IsDefault = true },
                new Category { Id = 4, Name = "Rozrywka", Icon = "movie", Color = "#8B5CF6", Type = CategoryType.Expense, IsDefault = true },
                new Category { Id = 5, Name = "Zdrowie", Icon = "healing", Color = "#10B981", Type = CategoryType.Expense, IsDefault = true },
                new Category { Id = 6, Name = "Ubrania", Icon = "checkroom", Color = "#EC4899", Type = CategoryType.Expense, IsDefault = true },
                new Category { Id = 7, Name = "Dom", Icon = "home", Color = "#6366F1", Type = CategoryType.Expense, IsDefault = true },
                new Category { Id = 8, Name = "Subskrypcje", Icon = "subscriptions", Color = "#14B8A6", Type = CategoryType.Expense, IsDefault = true },
                new Category { Id = 9, Name = "Wynagrodzenie", Icon = "payments", Color = "#22C55E", Type = CategoryType.Income, IsDefault = true },
                new Category { Id = 10, Name = "Freelance", Icon = "laptop", Color = "#06B6D4", Type = CategoryType.Income, IsDefault = true },
                new Category { Id = 11, Name = "Zwroty", Icon = "undo", Color = "#A3E635", Type = CategoryType.Income, IsDefault = true },
                new Category { Id = 12, Name = "Edukacja", Icon = "school", Color = "#A855F7", Type = CategoryType.Expense, IsDefault = true },
                new Category { Id = 13, Name = "Terapia", Icon = "psychology", Color = "#F97316", Type = CategoryType.Expense, IsDefault = true },
                new Category { Id = 14, Name = "Inne", Icon = "more-horiz", Color = "#6B7280", Type = CategoryType.Both, IsDefault = true }
            );

        // ===== TAGS =====
        mb.Entity<Tag>()
            .HasData(
                new Tag { Id = 1, Name = "pilne", HouseholdId = 1 },
                new Tag { Id = 2, Name = "oszczędność", HouseholdId = 1 },
                new Tag { Id = 3, Name = "zwrot", HouseholdId = 1 },
                new Tag { Id = 4, Name = "wakacje", HouseholdId = 1 },
                new Tag { Id = 5, Name = "prezent", HouseholdId = 1 },
                new Tag { Id = 6, Name = "zdrowie", HouseholdId = 1 },
                new Tag { Id = 7, Name = "dom", HouseholdId = 1 },
                new Tag { Id = 8, Name = "praca", HouseholdId = 1 }
            );

        // ===== RECURRING TRANSACTIONS =====
        mb.Entity<RecurringTransaction>()
            .HasData(
                new RecurringTransaction { Id = 1, Title = "Netflix", Amount = 49.99m, Type = TransactionType.Expense, Frequency = Frequency.Monthly, StartDate = new DateTime(2025, 1, 15), NextOccurrence = new DateTime(2025, 5, 15), IsActive = true, UserId = 1, CategoryId = 8, HouseholdId = 1 },
                new RecurringTransaction { Id = 2, Title = "Spotify", Amount = 29.99m, Type = TransactionType.Expense, Frequency = Frequency.Monthly, StartDate = new DateTime(2025, 2, 1), NextOccurrence = new DateTime(2025, 5, 1), IsActive = true, UserId = 2, CategoryId = 8, HouseholdId = 1 },
                new RecurringTransaction { Id = 3, Title = "Siłownia", Amount = 139.00m, Type = TransactionType.Expense, Frequency = Frequency.Monthly, StartDate = new DateTime(2025, 1, 5), NextOccurrence = new DateTime(2025, 5, 5), IsActive = true, UserId = 1, CategoryId = 5, HouseholdId = 1 },
                new RecurringTransaction { Id = 4, Title = "Terapia", Amount = 200.00m, Type = TransactionType.Expense, Frequency = Frequency.Weekly, StartDate = new DateTime(2025, 2, 10), NextOccurrence = new DateTime(2025, 4, 14), IsActive = true, UserId = 2, CategoryId = 13, HouseholdId = 1 },
                new RecurringTransaction { Id = 5, Title = "Kurs angielskiego", Amount = 280.00m, Type = TransactionType.Expense, Frequency = Frequency.Monthly, StartDate = new DateTime(2025, 1, 10), NextOccurrence = new DateTime(2025, 5, 10), IsActive = true, UserId = 1, CategoryId = 12, HouseholdId = 1 }
            );

        // ===== BILLS =====
        mb.Entity<Bill>()
            .HasData(
                new Bill { Id = 1, Name = "Prąd", Provider = "Tauron", DueDay = 15, EstimatedAmount = 280.00m, Icon = "bolt", Color = "#F59E0B", IsActive = true, HouseholdId = 1, CategoryId = 3 },
                new Bill { Id = 2, Name = "Gaz", Provider = "PGNiG", DueDay = 20, EstimatedAmount = 150.00m, Icon = "local-fire-department", Color = "#EF4444", IsActive = true, HouseholdId = 1, CategoryId = 3 },
                new Bill { Id = 3, Name = "Internet", Provider = "Orange", DueDay = 10, EstimatedAmount = 79.90m, Icon = "wifi", Color = "#6366F1", IsActive = true, HouseholdId = 1, CategoryId = 3 },
                new Bill { Id = 4, Name = "Woda", Provider = "MPWiK", DueDay = 25, EstimatedAmount = 95.00m, Icon = "water-drop", Color = "#3B82F6", IsActive = true, HouseholdId = 1, CategoryId = 3 },
                new Bill { Id = 5, Name = "Czynsz", Provider = "Spółdzielnia", DueDay = 1, EstimatedAmount = 2800.00m, Icon = "home", Color = "#6366F1", IsActive = true, HouseholdId = 1, CategoryId = 7 }
            );

        mb.Entity<BillPayment>()
            .HasData(
                new BillPayment { Id = 1, BillId = 1, Amount = 275.40m, DueDate = new DateTime(2025, 3, 15), PaidDate = new DateTime(2025, 3, 14), PaymentMethod = PaymentMethod.Transfer, Status = BillStatus.Paid },
                new BillPayment { Id = 2, BillId = 1, Amount = 290.30m, DueDate = new DateTime(2025, 4, 15), Status = BillStatus.Pending },
                new BillPayment { Id = 3, BillId = 2, Amount = 148.50m, DueDate = new DateTime(2025, 3, 20), PaidDate = new DateTime(2025, 3, 19), PaymentMethod = PaymentMethod.Transfer, Status = BillStatus.Paid },
                new BillPayment { Id = 4, BillId = 2, Amount = 152.00m, DueDate = new DateTime(2025, 4, 20), Status = BillStatus.Pending },
                new BillPayment { Id = 5, BillId = 3, Amount = 79.90m, DueDate = new DateTime(2025, 3, 10), PaidDate = new DateTime(2025, 3, 10), PaymentMethod = PaymentMethod.Card, Status = BillStatus.Paid },
                new BillPayment { Id = 6, BillId = 3, Amount = 79.90m, DueDate = new DateTime(2025, 4, 10), PaidDate = new DateTime(2025, 4, 10), PaymentMethod = PaymentMethod.Card, Status = BillStatus.Paid },
                new BillPayment { Id = 7, BillId = 4, Amount = 92.30m, DueDate = new DateTime(2025, 3, 25), PaidDate = new DateTime(2025, 3, 26), PaymentMethod = PaymentMethod.Transfer, Status = BillStatus.Paid },
                new BillPayment { Id = 8, BillId = 4, Amount = 98.00m, DueDate = new DateTime(2025, 4, 25), Status = BillStatus.Pending },
                new BillPayment { Id = 9, BillId = 5, Amount = 2800.00m, DueDate = new DateTime(2025, 4, 1), PaidDate = new DateTime(2025, 4, 1), PaymentMethod = PaymentMethod.Transfer, Status = BillStatus.Paid }
            );

        // ===== TRANSACTIONS =====
        mb.Entity<Transaction>()
            .HasData(
                new Transaction { Id = 1, Title = "Biedronka zakupy", Amount = 187.50m, Date = new DateTime(2025, 3, 2), Type = TransactionType.Expense, PaymentMethod = PaymentMethod.Card, IsShared = true, UserId = 1, CategoryId = 1, AccountId = 1, HouseholdId = 1, CreatedAt = new DateTime(2025, 3, 2) },
                new Transaction { Id = 2, Title = "Paliwo BP", Amount = 320.00m, Date = new DateTime(2025, 3, 3), Type = TransactionType.Expense, PaymentMethod = PaymentMethod.Card, IsShared = false, UserId = 1, CategoryId = 2, AccountId = 1, HouseholdId = 1, CreatedAt = new DateTime(2025, 3, 3) },
                new Transaction { Id = 3, Title = "Czynsz marzec", Amount = 2800.00m, Date = new DateTime(2025, 3, 1), Type = TransactionType.Expense, PaymentMethod = PaymentMethod.Transfer, IsShared = true, UserId = 1, CategoryId = 7, AccountId = 1, HouseholdId = 1, CreatedAt = new DateTime(2025, 3, 1) },
                new Transaction { Id = 4, Title = "Kino Helios", Amount = 56.00m, Date = new DateTime(2025, 3, 8), Type = TransactionType.Expense, PaymentMethod = PaymentMethod.BLIK, IsShared = true, UserId = 2, CategoryId = 4, AccountId = 4, HouseholdId = 1, CreatedAt = new DateTime(2025, 3, 8) },
                new Transaction { Id = 5, Title = "Rossmann", Amount = 89.90m, Date = new DateTime(2025, 3, 10), Type = TransactionType.Expense, PaymentMethod = PaymentMethod.Card, IsShared = false, UserId = 2, CategoryId = 5, AccountId = 4, HouseholdId = 1, CreatedAt = new DateTime(2025, 3, 10) },
                new Transaction { Id = 6, Title = "Wynagrodzenie WKOLOR", Amount = 7500.00m, Date = new DateTime(2025, 3, 10), Type = TransactionType.Income, PaymentMethod = PaymentMethod.Transfer, IsShared = false, UserId = 1, CategoryId = 9, AccountId = 1, HouseholdId = 1, CreatedAt = new DateTime(2025, 3, 10) },
                new Transaction { Id = 7, Title = "Wynagrodzenie Anna", Amount = 5200.00m, Date = new DateTime(2025, 3, 10), Type = TransactionType.Income, PaymentMethod = PaymentMethod.Transfer, IsShared = false, UserId = 2, CategoryId = 9, AccountId = 4, HouseholdId = 1, CreatedAt = new DateTime(2025, 3, 10) },
                new Transaction { Id = 8, Title = "Lidl zakupy", Amount = 234.30m, Date = new DateTime(2025, 3, 15), Type = TransactionType.Expense, PaymentMethod = PaymentMethod.Card, IsShared = true, UserId = 2, CategoryId = 1, AccountId = 4, HouseholdId = 1, CreatedAt = new DateTime(2025, 3, 15) },
                new Transaction { Id = 9, Title = "Netflix", Amount = 49.99m, Date = new DateTime(2025, 3, 15), Type = TransactionType.Expense, PaymentMethod = PaymentMethod.Card, IsShared = true, UserId = 1, CategoryId = 8, AccountId = 1, HouseholdId = 1, RecurringTransactionId = 1, CreatedAt = new DateTime(2025, 3, 15) },
                new Transaction { Id = 10, Title = "Zalando buty", Amount = 299.99m, Date = new DateTime(2025, 3, 18), Type = TransactionType.Expense, PaymentMethod = PaymentMethod.BLIK, IsShared = false, UserId = 2, CategoryId = 6, AccountId = 4, HouseholdId = 1, CreatedAt = new DateTime(2025, 3, 18) },
                new Transaction { Id = 11, Title = "IKEA półki", Amount = 459.00m, Date = new DateTime(2025, 3, 22), Type = TransactionType.Expense, PaymentMethod = PaymentMethod.Card, IsShared = true, UserId = 1, CategoryId = 7, AccountId = 1, HouseholdId = 1, CreatedAt = new DateTime(2025, 3, 22) },
                new Transaction { Id = 12, Title = "Freelance projekt", Amount = 2000.00m, Date = new DateTime(2025, 3, 25), Type = TransactionType.Income, PaymentMethod = PaymentMethod.Transfer, IsShared = false, UserId = 1, CategoryId = 10, AccountId = 1, HouseholdId = 1, CreatedAt = new DateTime(2025, 3, 25) },
                new Transaction { Id = 13, Title = "Siłownia marzec", Amount = 139.00m, Date = new DateTime(2025, 3, 5), Type = TransactionType.Expense, PaymentMethod = PaymentMethod.Transfer, IsShared = false, UserId = 1, CategoryId = 5, AccountId = 1, HouseholdId = 1, RecurringTransactionId = 3, CreatedAt = new DateTime(2025, 3, 5) },
                new Transaction { Id = 14, Title = "Spotify", Amount = 29.99m, Date = new DateTime(2025, 3, 1), Type = TransactionType.Expense, PaymentMethod = PaymentMethod.Card, IsShared = true, UserId = 2, CategoryId = 8, AccountId = 4, HouseholdId = 1, RecurringTransactionId = 2, CreatedAt = new DateTime(2025, 3, 1) },
                new Transaction { Id = 15, Title = "Kurs angielskiego", Amount = 280.00m, Date = new DateTime(2025, 3, 10), Type = TransactionType.Expense, PaymentMethod = PaymentMethod.Transfer, IsShared = false, UserId = 1, CategoryId = 12, AccountId = 1, HouseholdId = 1, RecurringTransactionId = 5, CreatedAt = new DateTime(2025, 3, 10) },
                new Transaction { Id = 16, Title = "Czynsz kwiecień", Amount = 2800.00m, Date = new DateTime(2025, 4, 1), Type = TransactionType.Expense, PaymentMethod = PaymentMethod.Transfer, IsShared = true, UserId = 1, CategoryId = 7, AccountId = 1, HouseholdId = 1, CreatedAt = new DateTime(2025, 4, 1) },
                new Transaction { Id = 17, Title = "Biedronka", Amount = 156.70m, Date = new DateTime(2025, 4, 2), Type = TransactionType.Expense, PaymentMethod = PaymentMethod.Card, IsShared = true, UserId = 1, CategoryId = 1, AccountId = 1, HouseholdId = 1, CreatedAt = new DateTime(2025, 4, 2) },
                new Transaction { Id = 18, Title = "Uber", Amount = 34.50m, Date = new DateTime(2025, 4, 3), Type = TransactionType.Expense, PaymentMethod = PaymentMethod.BLIK, IsShared = false, UserId = 2, CategoryId = 2, AccountId = 4, HouseholdId = 1, CreatedAt = new DateTime(2025, 4, 3) },
                new Transaction { Id = 19, Title = "Wynagrodzenie WKOLOR", Amount = 7500.00m, Date = new DateTime(2025, 4, 10), Type = TransactionType.Income, PaymentMethod = PaymentMethod.Transfer, IsShared = false, UserId = 1, CategoryId = 9, AccountId = 1, HouseholdId = 1, CreatedAt = new DateTime(2025, 4, 10) },
                new Transaction { Id = 20, Title = "Wynagrodzenie Anna", Amount = 5200.00m, Date = new DateTime(2025, 4, 10), Type = TransactionType.Income, PaymentMethod = PaymentMethod.Transfer, IsShared = false, UserId = 2, CategoryId = 9, AccountId = 4, HouseholdId = 1, CreatedAt = new DateTime(2025, 4, 10) },
                new Transaction { Id = 21, Title = "Restauracja Da Grasso", Amount = 112.00m, Date = new DateTime(2025, 4, 5), Type = TransactionType.Expense, PaymentMethod = PaymentMethod.Card, IsShared = true, UserId = 1, CategoryId = 4, AccountId = 1, HouseholdId = 1, CreatedAt = new DateTime(2025, 4, 5) },
                new Transaction { Id = 22, Title = "Allegro etui", Amount = 49.99m, Date = new DateTime(2025, 4, 6), Type = TransactionType.Expense, PaymentMethod = PaymentMethod.BLIK, IsShared = false, UserId = 1, CategoryId = 14, AccountId = 2, HouseholdId = 1, CreatedAt = new DateTime(2025, 4, 6) },
                new Transaction { Id = 23, Title = "Lidl zakupy", Amount = 198.40m, Date = new DateTime(2025, 4, 7), Type = TransactionType.Expense, PaymentMethod = PaymentMethod.Card, IsShared = true, UserId = 2, CategoryId = 1, AccountId = 4, HouseholdId = 1, CreatedAt = new DateTime(2025, 4, 7) },
                new Transaction { Id = 24, Title = "Zwrot Zalando", Amount = 149.99m, Date = new DateTime(2025, 4, 8), Type = TransactionType.Income, PaymentMethod = PaymentMethod.Transfer, IsShared = false, UserId = 2, CategoryId = 11, AccountId = 4, HouseholdId = 1, CreatedAt = new DateTime(2025, 4, 8) },
                new Transaction { Id = 25, Title = "Stacja Orlen", Amount = 290.00m, Date = new DateTime(2025, 4, 9), Type = TransactionType.Expense, PaymentMethod = PaymentMethod.Card, IsShared = false, UserId = 1, CategoryId = 2, AccountId = 1, HouseholdId = 1, CreatedAt = new DateTime(2025, 4, 9) },
                new Transaction { Id = 26, Title = "Dentysta", Amount = 350.00m, Date = new DateTime(2025, 4, 4), Type = TransactionType.Expense, PaymentMethod = PaymentMethod.Transfer, IsShared = false, UserId = 2, CategoryId = 5, AccountId = 4, HouseholdId = 1, CreatedAt = new DateTime(2025, 4, 4) },
                new Transaction { Id = 27, Title = "Castorama farby", Amount = 189.00m, Date = new DateTime(2025, 4, 6), Type = TransactionType.Expense, PaymentMethod = PaymentMethod.Card, IsShared = true, UserId = 1, CategoryId = 7, AccountId = 1, HouseholdId = 1, CreatedAt = new DateTime(2025, 4, 6) },
                new Transaction { Id = 28, Title = "H&M koszulki", Amount = 159.97m, Date = new DateTime(2025, 4, 8), Type = TransactionType.Expense, PaymentMethod = PaymentMethod.BLIK, IsShared = false, UserId = 1, CategoryId = 6, AccountId = 2, HouseholdId = 1, CreatedAt = new DateTime(2025, 4, 8) },
                new Transaction { Id = 29, Title = "Spotify kwiecień", Amount = 29.99m, Date = new DateTime(2025, 4, 1), Type = TransactionType.Expense, PaymentMethod = PaymentMethod.Card, IsShared = true, UserId = 2, CategoryId = 8, AccountId = 4, HouseholdId = 1, RecurringTransactionId = 2, CreatedAt = new DateTime(2025, 4, 1) },
                new Transaction { Id = 30, Title = "Żabka kawa", Amount = 12.99m, Date = new DateTime(2025, 4, 10), Type = TransactionType.Expense, PaymentMethod = PaymentMethod.BLIK, IsShared = false, UserId = 1, CategoryId = 1, AccountId = 3, HouseholdId = 1, CreatedAt = new DateTime(2025, 4, 10) },
                new Transaction { Id = 31, Title = "Terapia", Amount = 200.00m, Date = new DateTime(2025, 4, 7), Type = TransactionType.Expense, PaymentMethod = PaymentMethod.Transfer, IsShared = false, UserId = 2, CategoryId = 13, AccountId = 4, HouseholdId = 1, RecurringTransactionId = 4, CreatedAt = new DateTime(2025, 4, 7) },
                new Transaction { Id = 32, Title = "Netflix kwiecień", Amount = 49.99m, Date = new DateTime(2025, 4, 15), Type = TransactionType.Expense, PaymentMethod = PaymentMethod.Card, IsShared = true, UserId = 1, CategoryId = 8, AccountId = 1, HouseholdId = 1, RecurringTransactionId = 1, CreatedAt = new DateTime(2025, 4, 15) }
            );

        // ===== TRANSACTION TAGS (many-to-many) =====
        mb.Entity<TransactionTag>()
            .HasData(
                new { TransactionId = 3, TagId = 7 }, new { TransactionId = 3, TagId = 1 },
                new { TransactionId = 6, TagId = 8 }, new { TransactionId = 7, TagId = 8 },
                new { TransactionId = 10, TagId = 5 }, new { TransactionId = 11, TagId = 7 },
                new { TransactionId = 12, TagId = 8 }, new { TransactionId = 16, TagId = 7 },
                new { TransactionId = 16, TagId = 1 }, new { TransactionId = 19, TagId = 8 },
                new { TransactionId = 24, TagId = 3 }, new { TransactionId = 26, TagId = 6 },
                new { TransactionId = 27, TagId = 7 }, new { TransactionId = 31, TagId = 6 }
            );

        // ===== BUDGETS =====
        mb.Entity<Budget>()
            .HasData(
                new Budget { Id = 1, Amount = 1500.00m, Month = 4, Year = 2025, UserId = 1, CategoryId = 1, HouseholdId = 1 },
                new Budget { Id = 2, Amount = 500.00m, Month = 4, Year = 2025, UserId = 1, CategoryId = 2, HouseholdId = 1 },
                new Budget { Id = 3, Amount = 400.00m, Month = 4, Year = 2025, UserId = 1, CategoryId = 4, HouseholdId = 1 },
                new Budget { Id = 4, Amount = 300.00m, Month = 4, Year = 2025, UserId = 2, CategoryId = 6, HouseholdId = 1 },
                new Budget { Id = 5, Amount = 600.00m, Month = 4, Year = 2025, UserId = 1, CategoryId = 5, HouseholdId = 1 },
                new Budget { Id = 6, Amount = 3500.00m, Month = 4, Year = 2025, UserId = 1, CategoryId = 3, HouseholdId = 1 }
            );

        // ===== SAVINGS GOALS =====
        mb.Entity<SavingsGoal>()
            .HasData(
                new SavingsGoal { Id = 1, Name = "Wakacje Grecja", TargetAmount = 8000.00m, CurrentAmount = 3200.00m, Deadline = new DateTime(2025, 7, 1), Icon = "beach-access", Color = "#06B6D4", IsCompleted = false, HouseholdId = 1, CreatedAt = new DateTime(2025, 1, 15) },
                new SavingsGoal { Id = 2, Name = "Nowy laptop", TargetAmount = 5000.00m, CurrentAmount = 1800.00m, Deadline = new DateTime(2025, 9, 1), Icon = "laptop", Color = "#8B5CF6", IsCompleted = false, HouseholdId = 1, CreatedAt = new DateTime(2025, 2, 1) },
                new SavingsGoal { Id = 3, Name = "Poduszka bezpieczeństwa", TargetAmount = 15000.00m, CurrentAmount = 7500.00m, Deadline = null, Icon = "security", Color = "#10B981", IsCompleted = false, HouseholdId = 1, CreatedAt = new DateTime(2025, 1, 1) }
            );

        // ===== SHOPPING LISTS =====
        mb.Entity<ShoppingList>()
            .HasData(
                new ShoppingList { Id = 1, Name = "Biedronka sobota", IsCompleted = false, CreatedAt = new DateTime(2025, 4, 10), CreatedByUserId = 1, HouseholdId = 1 },
                new ShoppingList { Id = 2, Name = "IKEA meble do salonu", IsCompleted = false, CreatedAt = new DateTime(2025, 4, 8), CreatedByUserId = 2, HouseholdId = 1 },
                new ShoppingList { Id = 3, Name = "Apteka", IsCompleted = true, CreatedAt = new DateTime(2025, 4, 3), CompletedAt = new DateTime(2025, 4, 5), CreatedByUserId = 2, HouseholdId = 1 }
            );

        mb.Entity<ShoppingListMember>()
            .HasData(
                new { ShoppingListId = 1, UserId = 1, AddedAt = new DateTime(2025, 4, 10) },
                new { ShoppingListId = 1, UserId = 2, AddedAt = new DateTime(2025, 4, 10) },
                new { ShoppingListId = 2, UserId = 1, AddedAt = new DateTime(2025, 4, 8) },
                new { ShoppingListId = 2, UserId = 2, AddedAt = new DateTime(2025, 4, 8) },
                new { ShoppingListId = 3, UserId = 2, AddedAt = new DateTime(2025, 4, 3) }
            );

        mb.Entity<ShoppingItem>()
            .HasData(
                new ShoppingItem { Id = 1, Name = "Mleko 3.2%", Quantity = 2, EstimatedPrice = 4.50m, IsChecked = false, ShoppingListId = 1, CategoryId = 1 },
                new ShoppingItem { Id = 2, Name = "Chleb żytni", Quantity = 1, EstimatedPrice = 6.00m, IsChecked = false, ShoppingListId = 1, CategoryId = 1 },
                new ShoppingItem { Id = 3, Name = "Pomidory", Quantity = 1, EstimatedPrice = 12.00m, IsChecked = true, ShoppingListId = 1, CategoryId = 1 },
                new ShoppingItem { Id = 4, Name = "Ser żółty", Quantity = 1, EstimatedPrice = 15.00m, IsChecked = false, ShoppingListId = 1, CategoryId = 1 },
                new ShoppingItem { Id = 5, Name = "Jogurt naturalny", Quantity = 4, EstimatedPrice = 3.50m, IsChecked = false, ShoppingListId = 1, CategoryId = 1 },
                new ShoppingItem { Id = 6, Name = "Stolik kawowy", Quantity = 1, EstimatedPrice = 299.00m, IsChecked = false, ShoppingListId = 2, CategoryId = 7 },
                new ShoppingItem { Id = 7, Name = "Lampa stojąca", Quantity = 1, EstimatedPrice = 149.00m, IsChecked = false, ShoppingListId = 2, CategoryId = 7 },
                new ShoppingItem { Id = 8, Name = "Poduszki", Quantity = 2, EstimatedPrice = 45.00m, IsChecked = false, ShoppingListId = 2, CategoryId = 7 },
                new ShoppingItem { Id = 9, Name = "Witamina D", Quantity = 1, EstimatedPrice = 24.00m, IsChecked = true, ShoppingListId = 3, CategoryId = 5 },
                new ShoppingItem { Id = 10, Name = "Magnez", Quantity = 1, EstimatedPrice = 18.00m, IsChecked = true, ShoppingListId = 3, CategoryId = 5 }
            );

        // ===== EXPENSE SPLITS =====
        mb.Entity<ExpenseSplit>()
            .HasData(
                new ExpenseSplit { Id = 1, TransactionId = 3, PaidByUserId = 1, OwesToUserId = 2, Amount = 1400.00m, SplitType = SplitType.Equal, IsSettled = true, SettledAt = new DateTime(2025, 3, 5), CreatedAt = new DateTime(2025, 3, 1) },
                new ExpenseSplit { Id = 2, TransactionId = 11, PaidByUserId = 1, OwesToUserId = 2, Amount = 229.50m, SplitType = SplitType.Equal, IsSettled = true, SettledAt = new DateTime(2025, 3, 25), CreatedAt = new DateTime(2025, 3, 22) },
                new ExpenseSplit { Id = 3, TransactionId = 16, PaidByUserId = 1, OwesToUserId = 2, Amount = 1400.00m, SplitType = SplitType.Equal, IsSettled = false, CreatedAt = new DateTime(2025, 4, 1) },
                new ExpenseSplit { Id = 4, TransactionId = 27, PaidByUserId = 1, OwesToUserId = 2, Amount = 94.50m, SplitType = SplitType.Equal, IsSettled = false, CreatedAt = new DateTime(2025, 4, 6) },
                new ExpenseSplit { Id = 5, TransactionId = 4, PaidByUserId = 2, OwesToUserId = 1, Amount = 28.00m, SplitType = SplitType.Equal, IsSettled = true, SettledAt = new DateTime(2025, 3, 10), CreatedAt = new DateTime(2025, 3, 8) },
                new ExpenseSplit { Id = 6, TransactionId = 21, PaidByUserId = 1, OwesToUserId = 2, Amount = 56.00m, SplitType = SplitType.Equal, IsSettled = false, CreatedAt = new DateTime(2025, 4, 5) }
            );

        // ===== RECEIPTS =====
        mb.Entity<Receipt>()
            .HasData(
                new Receipt { Id = 1, TransactionId = 1, ImagePath = "/receipts/biedronka-2025-03-02.jpg", StoreName = "Biedronka", ReceiptDate = new DateTime(2025, 3, 2), CreatedAt = new DateTime(2025, 3, 2) },
                new Receipt { Id = 2, TransactionId = 11, ImagePath = "/receipts/ikea-2025-03-22.jpg", StoreName = "IKEA", ReceiptDate = new DateTime(2025, 3, 22), CreatedAt = new DateTime(2025, 3, 22) },
                new Receipt { Id = 3, TransactionId = 17, ImagePath = "/receipts/biedronka-2025-04-02.jpg", StoreName = "Biedronka", ReceiptDate = new DateTime(2025, 4, 2), CreatedAt = new DateTime(2025, 4, 2) },
                new Receipt { Id = 4, TransactionId = 27, ImagePath = "/receipts/castorama-2025-04-06.jpg", StoreName = "Castorama", ReceiptDate = new DateTime(2025, 4, 6), CreatedAt = new DateTime(2025, 4, 6) }
            );
    }
}
