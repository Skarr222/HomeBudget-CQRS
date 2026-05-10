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
        CancellationToken cancellationToken)
    {
        var transaction = await _context.Transactions
            .Include(transaction => transaction.TransactionTags)
            .FirstOrDefaultAsync(transaction => transaction.Id == command.Id, cancellationToken);

        if (transaction is null) return false;

        transaction.Title         = command.Title;
        transaction.Amount        = command.Amount;
        transaction.Date          = command.Date;
        transaction.Note          = command.Note;
        transaction.Type          = command.Type;
        transaction.PaymentMethod = command.PaymentMethod;
        transaction.IsShared      = command.IsShared;
        transaction.CategoryId    = command.CategoryId;
        transaction.AccountId     = command.AccountId;

        transaction.TransactionTags.Clear();
        if (command.TagIds?.Count > 0)
            foreach (var tagId in command.TagIds)
                transaction.TransactionTags.Add(new TransactionTag
                {
                    TransactionId = command.Id,
                    TagId         = tagId,
                });

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
