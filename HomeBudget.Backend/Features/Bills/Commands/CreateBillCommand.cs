using HomeBudget.Data.Context;
using HomeBudget.Data.Entities;
using MediatR;

namespace HomeBudget.Application.Bills.Commands;

public record CreateBillCommand(
    string Name,
    string Provider,
    int DueDay,
    decimal EstimatedAmount,
    string Icon,
    string Color,
    int HouseholdId,
    int CategoryId
) : IRequest<int>;

public class CreateBillCommandHandler : IRequestHandler<CreateBillCommand, int>
{
    private readonly HomeBudgetDbContext _context;

    public CreateBillCommandHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<int> Handle(
        CreateBillCommand command,
        CancellationToken cancellationToken)
    {
        var bill = new Bill
        {
            Name            = command.Name,
            Provider        = command.Provider,
            DueDay          = command.DueDay,
            EstimatedAmount = command.EstimatedAmount,
            Icon            = command.Icon,
            Color           = command.Color,
            IsActive        = true,
            HouseholdId     = command.HouseholdId,
            CategoryId      = command.CategoryId,
        };

        _context.Bills.Add(bill);
        await _context.SaveChangesAsync(cancellationToken);
        return bill.Id;
    }
}
