using HomeBudget.Data.Context;
using HomeBudget.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Application.Accounts.Commands;

public record UpdateAccountCommand(
    int Id,
    string Name,
    AccountType Type,
    decimal Balance,
    string Color,
    string Icon
) : IRequest<bool>;

public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, bool>
{
    private readonly HomeBudgetDbContext _context;

    public UpdateAccountCommandHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<bool> Handle(
        UpdateAccountCommand command,
        CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(account => account.Id == command.Id, cancellationToken);

        if (account is null) return false;

        account.Name    = command.Name;
        account.Type    = command.Type;
        account.Balance = command.Balance;
        account.Color   = command.Color;
        account.Icon    = command.Icon;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
