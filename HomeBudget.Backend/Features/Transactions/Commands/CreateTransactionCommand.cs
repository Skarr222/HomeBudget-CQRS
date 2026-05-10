using HomeBudget.Data.Context;
using HomeBudget.Data.Entities;
using HomeBudget.Data.Enums;
using MediatR;

namespace HomeBudget.Application.Transactions.Commands;

public record CreateTransactionCommand(
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
) : IRequest<int>;

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, int>
{
    private readonly HomeBudgetDbContext _context;

    public CreateTransactionCommandHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<int> Handle(
        CreateTransactionCommand command,
        CancellationToken cancellationToken)
    {
        var transaction = new Transaction
        {
            Title         = command.Title,
            Amount        = command.Amount,
            Date          = command.Date,
            Note          = command.Note,
            Type          = command.Type,
            PaymentMethod = command.PaymentMethod,
            IsShared      = command.IsShared,
            UserId        = command.UserId,
            CategoryId    = command.CategoryId,
            AccountId     = command.AccountId,
            HouseholdId   = command.HouseholdId,
            CreatedAt     = DateTime.UtcNow,
        };

        if (command.TagIds?.Count > 0)
            transaction.TransactionTags = command.TagIds
                .Select(tagId => new TransactionTag { TagId = tagId })
                .ToList();

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync(cancellationToken);
        return transaction.Id;
    }
}
