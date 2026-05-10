using HomeBudget.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Application.Transactions.Commands;

public record DeleteTransactionCommand(int Id) : IRequest<bool>;

public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand, bool>
{
    private readonly HomeBudgetDbContext _context;

    public DeleteTransactionCommandHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<bool> Handle(
        DeleteTransactionCommand command,
        CancellationToken cancellationToken
    )
    {
        var transaction = await _context.Transactions.FirstOrDefaultAsync(
            transaction => transaction.Id == command.Id,
            cancellationToken
        );

        if (transaction is null)
            return false;

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
