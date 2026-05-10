using HomeBudget.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Application.Accounts.Commands;

public record DeleteAccountCommand(int Id) : IRequest<bool>;

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, bool>
{
    private readonly HomeBudgetDbContext _context;

    public DeleteAccountCommandHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<bool> Handle(
        DeleteAccountCommand command,
        CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(account => account.Id == command.Id, cancellationToken);

        if (account is null) return false;

        _context.Accounts.Remove(account);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
