using HomeBudget.Data.Enums;

namespace HomeBudget.Application.Accounts;

public record AccountDto(
    int Id,
    string Name,
    AccountType Type,
    decimal Balance,
    string Color,
    string Icon,
    int UserId,
    string UserName
);

public record CreateAccountDto(
    string Name,
    AccountType Type,
    decimal Balance,
    string Color,
    string Icon,
    int UserId
);

public record UpdateAccountDto(
    string Name,
    AccountType Type,
    decimal Balance,
    string Color,
    string Icon
);
