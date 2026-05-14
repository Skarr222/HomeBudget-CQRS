using HomeBudget.Application.Transactions;
using HomeBudget.Data.Entities;

namespace HomeBudget.Application.Transactions.Maps;

public static class TransactionMappings
{
    public static TransactionDto ToDto(Transaction transaction) =>
        new TransactionDto(
            transaction.Id,
            transaction.Title,
            transaction.Amount,
            transaction.Date,
            transaction.Note,
            transaction.Type,
            transaction.PaymentMethod,
            transaction.IsShared,
            transaction.User.FirstName + " " + transaction.User.LastName,
            transaction.UserId,
            transaction.Category.Name,
            transaction.Category.Icon,
            transaction.Category.Color,
            transaction.CategoryId,
            transaction.Account.Name,
            transaction.AccountId,
            transaction.TransactionTags.Select(tt => tt.Tag.Name).ToList(),
            transaction.Receipt != null,
            transaction.Splits.Any(),
            transaction.CreatedAt
        );
}
