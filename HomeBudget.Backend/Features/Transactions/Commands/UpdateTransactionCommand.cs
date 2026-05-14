using HomeBudget.Data.Context;
using HomeBudget.Data.Entities;
using HomeBudget.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Application.Transactions.Commands;

public record UpdateTransactionCommand(
    int Id,
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
) : IRequest<bool>;

public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand, bool>
{
    private readonly HomeBudgetDbContext _context;

    public UpdateTransactionCommandHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<bool> Handle(
        UpdateTransactionCommand command,
        CancellationToken cancellationToken
    )
    {
        var transaction = await _context
            .Transactions.Include(transaction => transaction.TransactionTags)
            .FirstOrDefaultAsync(transaction => transaction.Id == command.Id, cancellationToken);

        if (transaction is null)
            return false;

        // Reverse old balance effect
        var oldAccountId = transaction.AccountId;
        var oldAccount = await _context.Accounts.FindAsync(
            new object[] { oldAccountId },
            cancellationToken
        );
        if (oldAccount is not null)
        {
            if (transaction.Type == TransactionType.Expense)
                oldAccount.Balance += transaction.Amount;
            else
                oldAccount.Balance -= transaction.Amount;
        }

        transaction.Title = command.Title;
        transaction.Amount = command.Amount;
        transaction.Date = command.Date;
        transaction.Note = command.Note;
        transaction.Type = command.Type;
        transaction.PaymentMethod = command.PaymentMethod;
        transaction.IsShared = command.IsShared;
        transaction.CategoryId = command.CategoryId;
        transaction.AccountId = command.AccountId;

        // Apply new balance effect
        var newAccount =
            oldAccountId == command.AccountId
                ? oldAccount
                : await _context.Accounts.FindAsync(
                    new object[] { command.AccountId },
                    cancellationToken
                );
        if (newAccount is not null)
        {
            if (command.Type == TransactionType.Expense)
                newAccount.Balance -= command.Amount;
            else
                newAccount.Balance += command.Amount;
        }

        transaction.TransactionTags.Clear();
        if (command.TagIds?.Count > 0)
            foreach (var tagId in command.TagIds)
                transaction.TransactionTags.Add(
                    new TransactionTag { TransactionId = command.Id, TagId = tagId }
                );

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
