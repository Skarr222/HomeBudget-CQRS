using HomeBudget.Data.Context;
using HomeBudget.Data.Enums;
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

        // Reverse balance effect before deleting
        var account = await _context.Accounts.FindAsync(
            new object[] { transaction.AccountId },
            cancellationToken
        );
        if (account is not null)
        {
            if (transaction.Type == TransactionType.Expense)
                account.Balance += transaction.Amount;
            else
                account.Balance -= transaction.Amount;
        }

        // Reset linked bill payment back to Pending
        var billPayment = await _context.BillPayments.FirstOrDefaultAsync(
            billPayment => billPayment.TransactionId == command.Id,
            cancellationToken
        );
        if (billPayment is not null)
        {
            billPayment.Status = BillStatus.Pending;
            billPayment.PaidDate = null;
            billPayment.PaymentMethod = null;
            billPayment.TransactionId = null;
        }

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
