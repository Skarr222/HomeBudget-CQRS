using HomeBudget.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Application.Bills.Commands;

public record UpdateBillCommand(
    int Id,
    string Name,
    string Provider,
    int DueDay,
    decimal EstimatedAmount,
    string Icon,
    string Color,
    bool IsActive
) : IRequest<bool>;

public class UpdateBillCommandHandler : IRequestHandler<UpdateBillCommand, bool>
{
    private readonly HomeBudgetDbContext _context;

    public UpdateBillCommandHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<bool> Handle(
        UpdateBillCommand command,
        CancellationToken cancellationToken)
    {
        var bill = await _context.Bills
            .FirstOrDefaultAsync(bill => bill.Id == command.Id, cancellationToken);

        if (bill is null) return false;

        bill.Name            = command.Name;
        bill.Provider        = command.Provider;
        bill.DueDay          = command.DueDay;
        bill.EstimatedAmount = command.EstimatedAmount;
        bill.Icon            = command.Icon;
        bill.Color           = command.Color;
        bill.IsActive        = command.IsActive;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
