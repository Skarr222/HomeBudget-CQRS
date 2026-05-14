using HomeBudget.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Application.Bills.Commands;

public record DeleteBillCommand(int Id) : IRequest<bool>;

public class DeleteBillCommandHandler : IRequestHandler<DeleteBillCommand, bool>
{
    private readonly HomeBudgetDbContext _context;

    public DeleteBillCommandHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<bool> Handle(DeleteBillCommand command, CancellationToken cancellationToken)
    {
        var bill = await _context.Bills.FirstOrDefaultAsync(
            bill => bill.Id == command.Id,
            cancellationToken
        );

        if (bill is null)
            return false;

        _context.Bills.Remove(bill);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
