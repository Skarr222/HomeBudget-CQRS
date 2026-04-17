using HomeBudget.Data.Enums;

namespace HomeBudget.Application.Transactions;

public record TransactionDto(
    int Id,
    string Title,
    decimal Amount,
    DateTime Date,
    string? Note,
    TransactionType Type,
    PaymentMethod PaymentMethod,
    bool IsShared,
    string UserName,
    int UserId,
    string CategoryName,
    string CategoryIcon,
    string CategoryColor,
    int CategoryId,
    string AccountName,
    int AccountId,
    List<string> Tags,
    bool HasReceipt,
    bool HasSplit,
    DateTime CreatedAt
);

public record CreateTransactionDto(
    string Title,
    decimal Amount,
    DateTime Date,
    string? Note,
    TransactionType Type,
    PaymentMethod PaymentMethod,
    bool IsShared,
    int UserId,
    int CategoryId,
    int AccountId,
    int? HouseholdId,
    List<int>? TagIds
);

public record UpdateTransactionDto(
    string Title,
    decimal Amount,
    DateTime Date,
    string? Note,
    TransactionType Type,
    PaymentMethod PaymentMethod,
    bool IsShared,
    int CategoryId,
    int AccountId,
    List<int>? TagIds
);
