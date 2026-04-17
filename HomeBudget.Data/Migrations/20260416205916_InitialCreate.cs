using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HomeBudget.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Households",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InviteCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Households", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    HouseholdId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SavingsGoals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CurrentAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    HouseholdId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavingsGoals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavingsGoals_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HouseholdId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HouseholdMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HouseholdId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseholdMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HouseholdMembers_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HouseholdMembers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    HouseholdId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingLists_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingLists_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DueDay = table.Column<int>(type: "int", nullable: false),
                    EstimatedAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    HouseholdId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bills_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bills_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Budgets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    HouseholdId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budgets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Budgets_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Budgets_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Budgets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecurringTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Frequency = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextOccurrence = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    HouseholdId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurringTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecurringTransactions_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecurringTransactions_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecurringTransactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    EstimatedPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    IsChecked = table.Column<bool>(type: "bit", nullable: false),
                    ShoppingListId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingItems_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ShoppingItems_ShoppingLists_ShoppingListId",
                        column: x => x.ShoppingListId,
                        principalTable: "ShoppingLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingListMembers",
                columns: table => new
                {
                    ShoppingListId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingListMembers", x => new { x.ShoppingListId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ShoppingListMembers_ShoppingLists_ShoppingListId",
                        column: x => x.ShoppingListId,
                        principalTable: "ShoppingLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingListMembers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    IsShared = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    HouseholdId = table.Column<int>(type: "int", nullable: true),
                    RecurringTransactionId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transactions_RecurringTransactions_RecurringTransactionId",
                        column: x => x.RecurringTransactionId,
                        principalTable: "RecurringTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Transactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BillPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BillId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentMethod = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillPayments_Bills_BillId",
                        column: x => x.BillId,
                        principalTable: "Bills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillPayments_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseSplits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    PaidByUserId = table.Column<int>(type: "int", nullable: false),
                    OwesToUserId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SplitType = table.Column<int>(type: "int", nullable: false),
                    IsSettled = table.Column<bool>(type: "bit", nullable: false),
                    SettledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseSplits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseSplits_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpenseSplits_Users_OwesToUserId",
                        column: x => x.OwesToUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpenseSplits_Users_PaidByUserId",
                        column: x => x.PaidByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Receipts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StoreName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiptDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receipts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Receipts_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionTags",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTags", x => new { x.TransactionId, x.TagId });
                    table.ForeignKey(
                        name: "FK_TransactionTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionTags_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Color", "HouseholdId", "Icon", "IsDefault", "Name", "Type" },
                values: new object[,]
                {
                    { 1, "#EF4444", null, "restaurant", true, "Jedzenie", 0 },
                    { 2, "#3B82F6", null, "directions-car", true, "Transport", 0 },
                    { 3, "#F59E0B", null, "receipt", true, "Rachunki", 0 },
                    { 4, "#8B5CF6", null, "movie", true, "Rozrywka", 0 },
                    { 5, "#10B981", null, "healing", true, "Zdrowie", 0 },
                    { 6, "#EC4899", null, "checkroom", true, "Ubrania", 0 },
                    { 7, "#6366F1", null, "home", true, "Dom", 0 },
                    { 8, "#14B8A6", null, "subscriptions", true, "Subskrypcje", 0 },
                    { 9, "#22C55E", null, "payments", true, "Wynagrodzenie", 1 },
                    { 10, "#06B6D4", null, "laptop", true, "Freelance", 1 },
                    { 11, "#A3E635", null, "undo", true, "Zwroty", 1 },
                    { 12, "#A855F7", null, "school", true, "Edukacja", 0 },
                    { 13, "#F97316", null, "psychology", true, "Terapia", 0 },
                    { 14, "#6B7280", null, "more-horiz", true, "Inne", 2 }
                });

            migrationBuilder.InsertData(
                table: "Households",
                columns: new[] { "Id", "CreatedAt", "InviteCode", "Name" },
                values: new object[] { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "HOME2025", "Nasz Dom" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Currency", "Email", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "PLN", "oskar@homebudget.pl", "Oskar", "Gomza" },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "PLN", "anna@homebudget.pl", "Anna", "Kowalska" }
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Balance", "Color", "CreatedAt", "Icon", "Name", "Type", "UserId" },
                values: new object[,]
                {
                    { 1, 4850.30m, "#0077C8", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "account-balance", "PKO BP", 0, 1 },
                    { 2, 1230.50m, "#0075EB", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "credit-card", "Revolut", 0, 1 },
                    { 3, 320.00m, "#10B981", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "payments", "Gotówka", 2, 1 },
                    { 4, 3420.00m, "#E3000F", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "account-balance", "mBank", 0, 2 },
                    { 5, 12000.00m, "#F59E0B", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "savings", "Oszczędności", 1, 1 }
                });

            migrationBuilder.InsertData(
                table: "Bills",
                columns: new[] { "Id", "CategoryId", "Color", "DueDay", "EstimatedAmount", "HouseholdId", "Icon", "IsActive", "Name", "Provider" },
                values: new object[,]
                {
                    { 1, 3, "#F59E0B", 15, 280.00m, 1, "bolt", true, "Prąd", "Tauron" },
                    { 2, 3, "#EF4444", 20, 150.00m, 1, "local-fire-department", true, "Gaz", "PGNiG" },
                    { 3, 3, "#6366F1", 10, 79.90m, 1, "wifi", true, "Internet", "Orange" },
                    { 4, 3, "#3B82F6", 25, 95.00m, 1, "water-drop", true, "Woda", "MPWiK" },
                    { 5, 7, "#6366F1", 1, 2800.00m, 1, "home", true, "Czynsz", "Spółdzielnia" }
                });

            migrationBuilder.InsertData(
                table: "Budgets",
                columns: new[] { "Id", "Amount", "CategoryId", "HouseholdId", "Month", "UserId", "Year" },
                values: new object[,]
                {
                    { 1, 1500.00m, 1, 1, 4, 1, 2025 },
                    { 2, 500.00m, 2, 1, 4, 1, 2025 },
                    { 3, 400.00m, 4, 1, 4, 1, 2025 },
                    { 4, 300.00m, 6, 1, 4, 2, 2025 },
                    { 5, 600.00m, 5, 1, 4, 1, 2025 },
                    { 6, 3500.00m, 3, 1, 4, 1, 2025 }
                });

            migrationBuilder.InsertData(
                table: "HouseholdMembers",
                columns: new[] { "Id", "HouseholdId", "JoinedAt", "Role", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 1 },
                    { 2, 1, new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "RecurringTransactions",
                columns: new[] { "Id", "Amount", "CategoryId", "EndDate", "Frequency", "HouseholdId", "IsActive", "NextOccurrence", "StartDate", "Title", "Type", "UserId" },
                values: new object[,]
                {
                    { 1, 49.99m, 8, null, 2, 1, true, new DateTime(2025, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Netflix", 0, 1 },
                    { 2, 29.99m, 8, null, 2, 1, true, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spotify", 0, 2 },
                    { 3, 139.00m, 5, null, 2, 1, true, new DateTime(2025, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Siłownia", 0, 1 },
                    { 4, 200.00m, 13, null, 1, 1, true, new DateTime(2025, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Terapia", 0, 2 },
                    { 5, 280.00m, 12, null, 2, 1, true, new DateTime(2025, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kurs angielskiego", 0, 1 }
                });

            migrationBuilder.InsertData(
                table: "SavingsGoals",
                columns: new[] { "Id", "Color", "CreatedAt", "CurrentAmount", "Deadline", "HouseholdId", "Icon", "IsCompleted", "Name", "TargetAmount" },
                values: new object[,]
                {
                    { 1, "#06B6D4", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 3200.00m, new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "beach-access", false, "Wakacje Grecja", 8000.00m },
                    { 2, "#8B5CF6", new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1800.00m, new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "laptop", false, "Nowy laptop", 5000.00m },
                    { 3, "#10B981", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7500.00m, null, 1, "security", false, "Poduszka bezpieczeństwa", 15000.00m }
                });

            migrationBuilder.InsertData(
                table: "ShoppingLists",
                columns: new[] { "Id", "CompletedAt", "CreatedAt", "CreatedByUserId", "HouseholdId", "IsCompleted", "Name" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, false, "Biedronka sobota" },
                    { 2, null, new DateTime(2025, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, false, "IKEA meble do salonu" },
                    { 3, new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, true, "Apteka" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "HouseholdId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "pilne" },
                    { 2, 1, "oszczędność" },
                    { 3, 1, "zwrot" },
                    { 4, 1, "wakacje" },
                    { 5, 1, "prezent" },
                    { 6, 1, "zdrowie" },
                    { 7, 1, "dom" },
                    { 8, 1, "praca" }
                });

            migrationBuilder.InsertData(
                table: "BillPayments",
                columns: new[] { "Id", "Amount", "BillId", "DueDate", "PaidDate", "PaymentMethod", "Status", "TransactionId" },
                values: new object[,]
                {
                    { 1, 275.40m, 1, new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, null },
                    { 2, 290.30m, 1, new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 0, null },
                    { 3, 148.50m, 2, new DateTime(2025, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, null },
                    { 4, 152.00m, 2, new DateTime(2025, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 0, null },
                    { 5, 79.90m, 3, new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, null },
                    { 6, 79.90m, 3, new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, null },
                    { 7, 92.30m, 4, new DateTime(2025, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, null },
                    { 8, 98.00m, 4, new DateTime(2025, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 0, null },
                    { 9, 2800.00m, 5, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, null }
                });

            migrationBuilder.InsertData(
                table: "ShoppingItems",
                columns: new[] { "Id", "CategoryId", "EstimatedPrice", "IsChecked", "Name", "Quantity", "ShoppingListId" },
                values: new object[,]
                {
                    { 1, 1, 4.50m, false, "Mleko 3.2%", 2, 1 },
                    { 2, 1, 6.00m, false, "Chleb żytni", 1, 1 },
                    { 3, 1, 12.00m, true, "Pomidory", 1, 1 },
                    { 4, 1, 15.00m, false, "Ser żółty", 1, 1 },
                    { 5, 1, 3.50m, false, "Jogurt naturalny", 4, 1 },
                    { 6, 7, 299.00m, false, "Stolik kawowy", 1, 2 },
                    { 7, 7, 149.00m, false, "Lampa stojąca", 1, 2 },
                    { 8, 7, 45.00m, false, "Poduszki", 2, 2 },
                    { 9, 5, 24.00m, true, "Witamina D", 1, 3 },
                    { 10, 5, 18.00m, true, "Magnez", 1, 3 }
                });

            migrationBuilder.InsertData(
                table: "ShoppingListMembers",
                columns: new[] { "ShoppingListId", "UserId", "AddedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 1, 2, new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, new DateTime(2025, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2, new DateTime(2025, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 2, new DateTime(2025, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "AccountId", "Amount", "CategoryId", "CreatedAt", "Date", "HouseholdId", "IsShared", "Note", "PaymentMethod", "RecurringTransactionId", "Title", "Type", "UserId" },
                values: new object[,]
                {
                    { 1, 1, 187.50m, 1, new DateTime(2025, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, null, 1, null, "Biedronka zakupy", 0, 1 },
                    { 2, 1, 320.00m, 2, new DateTime(2025, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, false, null, 1, null, "Paliwo BP", 0, 1 },
                    { 3, 1, 2800.00m, 7, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, null, 2, null, "Czynsz marzec", 0, 1 },
                    { 4, 4, 56.00m, 4, new DateTime(2025, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, null, 3, null, "Kino Helios", 0, 2 },
                    { 5, 4, 89.90m, 5, new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, false, null, 1, null, "Rossmann", 0, 2 },
                    { 6, 1, 7500.00m, 9, new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, false, null, 2, null, "Wynagrodzenie WKOLOR", 1, 1 },
                    { 7, 4, 5200.00m, 9, new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, false, null, 2, null, "Wynagrodzenie Anna", 1, 2 },
                    { 8, 4, 234.30m, 1, new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, null, 1, null, "Lidl zakupy", 0, 2 },
                    { 9, 1, 49.99m, 8, new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, null, 1, 1, "Netflix", 0, 1 },
                    { 10, 4, 299.99m, 6, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, false, null, 3, null, "Zalando buty", 0, 2 },
                    { 11, 1, 459.00m, 7, new DateTime(2025, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, null, 1, null, "IKEA półki", 0, 1 },
                    { 12, 1, 2000.00m, 10, new DateTime(2025, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, false, null, 2, null, "Freelance projekt", 1, 1 },
                    { 13, 1, 139.00m, 5, new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, false, null, 2, 3, "Siłownia marzec", 0, 1 },
                    { 14, 4, 29.99m, 8, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, null, 1, 2, "Spotify", 0, 2 },
                    { 15, 1, 280.00m, 12, new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, false, null, 2, 5, "Kurs angielskiego", 0, 1 },
                    { 16, 1, 2800.00m, 7, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, null, 2, null, "Czynsz kwiecień", 0, 1 },
                    { 17, 1, 156.70m, 1, new DateTime(2025, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, null, 1, null, "Biedronka", 0, 1 },
                    { 18, 4, 34.50m, 2, new DateTime(2025, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, false, null, 3, null, "Uber", 0, 2 },
                    { 19, 1, 7500.00m, 9, new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, false, null, 2, null, "Wynagrodzenie WKOLOR", 1, 1 },
                    { 20, 4, 5200.00m, 9, new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, false, null, 2, null, "Wynagrodzenie Anna", 1, 2 },
                    { 21, 1, 112.00m, 4, new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, null, 1, null, "Restauracja Da Grasso", 0, 1 },
                    { 22, 2, 49.99m, 14, new DateTime(2025, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, false, null, 3, null, "Allegro etui", 0, 1 },
                    { 23, 4, 198.40m, 1, new DateTime(2025, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, null, 1, null, "Lidl zakupy", 0, 2 },
                    { 24, 4, 149.99m, 11, new DateTime(2025, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, false, null, 2, null, "Zwrot Zalando", 1, 2 },
                    { 25, 1, 290.00m, 2, new DateTime(2025, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, false, null, 1, null, "Stacja Orlen", 0, 1 },
                    { 26, 4, 350.00m, 5, new DateTime(2025, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, false, null, 2, null, "Dentysta", 0, 2 },
                    { 27, 1, 189.00m, 7, new DateTime(2025, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, null, 1, null, "Castorama farby", 0, 1 },
                    { 28, 2, 159.97m, 6, new DateTime(2025, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, false, null, 3, null, "H&M koszulki", 0, 1 },
                    { 29, 4, 29.99m, 8, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, null, 1, 2, "Spotify kwiecień", 0, 2 },
                    { 30, 3, 12.99m, 1, new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, false, null, 3, null, "Żabka kawa", 0, 1 },
                    { 31, 4, 200.00m, 13, new DateTime(2025, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, false, null, 2, 4, "Terapia", 0, 2 },
                    { 32, 1, 49.99m, 8, new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, null, 1, 1, "Netflix kwiecień", 0, 1 }
                });

            migrationBuilder.InsertData(
                table: "ExpenseSplits",
                columns: new[] { "Id", "Amount", "CreatedAt", "IsSettled", "OwesToUserId", "PaidByUserId", "SettledAt", "SplitType", "TransactionId" },
                values: new object[,]
                {
                    { 1, 1400.00m, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 2, 1, new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 3 },
                    { 2, 229.50m, new DateTime(2025, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 2, 1, new DateTime(2025, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 11 },
                    { 3, 1400.00m, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 2, 1, null, 0, 16 },
                    { 4, 94.50m, new DateTime(2025, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 2, 1, null, 0, 27 },
                    { 5, 28.00m, new DateTime(2025, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, 2, new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 4 },
                    { 6, 56.00m, new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 2, 1, null, 0, 21 }
                });

            migrationBuilder.InsertData(
                table: "Receipts",
                columns: new[] { "Id", "CreatedAt", "ImagePath", "Notes", "ReceiptDate", "StoreName", "TransactionId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "/receipts/biedronka-2025-03-02.jpg", null, new DateTime(2025, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Biedronka", 1 },
                    { 2, new DateTime(2025, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "/receipts/ikea-2025-03-22.jpg", null, new DateTime(2025, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "IKEA", 11 },
                    { 3, new DateTime(2025, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "/receipts/biedronka-2025-04-02.jpg", null, new DateTime(2025, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Biedronka", 17 },
                    { 4, new DateTime(2025, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "/receipts/castorama-2025-04-06.jpg", null, new DateTime(2025, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Castorama", 27 }
                });

            migrationBuilder.InsertData(
                table: "TransactionTags",
                columns: new[] { "TagId", "TransactionId" },
                values: new object[,]
                {
                    { 1, 3 },
                    { 7, 3 },
                    { 8, 6 },
                    { 8, 7 },
                    { 5, 10 },
                    { 7, 11 },
                    { 8, 12 },
                    { 1, 16 },
                    { 7, 16 },
                    { 8, 19 },
                    { 3, 24 },
                    { 6, 26 },
                    { 7, 27 },
                    { 6, 31 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BillPayments_BillId",
                table: "BillPayments",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_BillPayments_TransactionId",
                table: "BillPayments",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_CategoryId",
                table: "Bills",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_HouseholdId",
                table: "Bills",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_CategoryId",
                table: "Budgets",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_HouseholdId",
                table: "Budgets",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_UserId",
                table: "Budgets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_HouseholdId",
                table: "Categories",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseSplits_OwesToUserId",
                table: "ExpenseSplits",
                column: "OwesToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseSplits_PaidByUserId",
                table: "ExpenseSplits",
                column: "PaidByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseSplits_TransactionId",
                table: "ExpenseSplits",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_HouseholdMembers_HouseholdId",
                table: "HouseholdMembers",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_HouseholdMembers_UserId",
                table: "HouseholdMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_TransactionId",
                table: "Receipts",
                column: "TransactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecurringTransactions_CategoryId",
                table: "RecurringTransactions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringTransactions_HouseholdId",
                table: "RecurringTransactions",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringTransactions_UserId",
                table: "RecurringTransactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SavingsGoals_HouseholdId",
                table: "SavingsGoals",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingItems_CategoryId",
                table: "ShoppingItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingItems_ShoppingListId",
                table: "ShoppingItems",
                column: "ShoppingListId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListMembers_UserId",
                table: "ShoppingListMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingLists_CreatedByUserId",
                table: "ShoppingLists",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingLists_HouseholdId",
                table: "ShoppingLists",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_HouseholdId",
                table: "Tags",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoryId",
                table: "Transactions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_HouseholdId",
                table: "Transactions",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_RecurringTransactionId",
                table: "Transactions",
                column: "RecurringTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTags_TagId",
                table: "TransactionTags",
                column: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillPayments");

            migrationBuilder.DropTable(
                name: "Budgets");

            migrationBuilder.DropTable(
                name: "ExpenseSplits");

            migrationBuilder.DropTable(
                name: "HouseholdMembers");

            migrationBuilder.DropTable(
                name: "Receipts");

            migrationBuilder.DropTable(
                name: "SavingsGoals");

            migrationBuilder.DropTable(
                name: "ShoppingItems");

            migrationBuilder.DropTable(
                name: "ShoppingListMembers");

            migrationBuilder.DropTable(
                name: "TransactionTags");

            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "ShoppingLists");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "RecurringTransactions");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Households");
        }
    }
}
