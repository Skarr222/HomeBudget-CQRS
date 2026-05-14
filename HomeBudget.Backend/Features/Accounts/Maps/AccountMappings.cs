using HomeBudget.Application.Accounts;
using HomeBudget.Data.Entities;

namespace HomeBudget.Application.Accounts.Maps;

public static class AccountMappings
{
    public static AccountDto ToDto(Account account) =>
        new AccountDto(
            account.Id,
            account.Name,
            account.Type,
            account.Balance,
            account.Color,
            account.Icon,
            account.UserId,
            account.User.FirstName + " " + account.User.LastName
        );
}
