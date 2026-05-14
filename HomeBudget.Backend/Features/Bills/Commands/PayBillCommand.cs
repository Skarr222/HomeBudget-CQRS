using HomeBudget.Data.Context;
using HomeBudget.Data.Entities;
using HomeBudget.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Application.Bills.Commands;

public record PayBillCommand(
    int BillId,
    decimal Amount,
    PaymentMethod PaymentMethod,
    int AccountId,
    int UserId,
    DateTime PaidDate
) : IRequest<int>;

public class PayBillCommandHandler : IRequestHandler<PayBillCommand, int>
{
    private readonly HomeBudgetDbContext _context;

    public PayBillCommandHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<int> Handle(PayBillCommand command, CancellationToken cancellationToken)
    {
        var bill =
            await _context
                .Bills.Include(bill => bill.Payments)
                .FirstOrDefaultAsync(bill => bill.Id == command.BillId, cancellationToken)
            ?? throw new InvalidOperationException($"Bill {command.BillId} not found");

        // Create transaction for this payment
        var transaction = new Transaction
        {
            Title = $"Rachunek: {bill.Name}",
            Amount = command.Amount,
            Date = command.PaidDate,
            Type = TransactionType.Expense,
            PaymentMethod = command.PaymentMethod,
            IsShared = false,
            UserId = command.UserId,
            CategoryId = bill.CategoryId,
            AccountId = command.AccountId,
            HouseholdId = bill.HouseholdId,
            CreatedAt = DateTime.UtcNow,
        };
        _context.Transactions.Add(transaction);

        // Update account balance
        var account = await _context.Accounts.FindAsync([command.AccountId], cancellationToken);
        if (account is not null)
            account.Balance -= command.Amount;

        await _context.SaveChangesAsync(cancellationToken);

        // Find existing pending payment for current month or create one
        var now = command.PaidDate;

        var payment = bill
            .Payments.Where(billPayment =>
                billPayment.Status != BillStatus.Paid
                && billPayment.DueDate.Year == now.Year
                && billPayment.DueDate.Month == now.Month
            )
            .OrderBy(billPayment => billPayment.DueDate)
            .FirstOrDefault();

        if (payment is null)
        {
            payment = new BillPayment
            {
                BillId = command.BillId,
                Amount = command.Amount,
                DueDate = new DateTime(now.Year, now.Month, bill.DueDay),
                Status = BillStatus.Paid,
                PaidDate = command.PaidDate,
                PaymentMethod = command.PaymentMethod,
                TransactionId = transaction.Id,
            };
            _context.BillPayments.Add(payment);
        }
        else
        {
            payment.Amount = command.Amount;
            payment.Status = BillStatus.Paid;
            payment.PaidDate = command.PaidDate;
            payment.PaymentMethod = command.PaymentMethod;
            payment.TransactionId = transaction.Id;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return transaction.Id;
    }
}
