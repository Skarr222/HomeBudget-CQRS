using HomeBudget.Data.Context;
using HomeBudget.Data.Entities;
using HomeBudget.Data.Enums;
using MediatR;

namespace HomeBudget.Application.Accounts.Commands;

public record CreateAccountCommand(
    string Name,
    AccountType Type,
    decimal Balance,
    string Color,
    string Icon,
    int UserId
) : IRequest<int>;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, int>
{
    private readonly HomeBudgetDbContext _context;

    public CreateAccountCommandHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<int> Handle(
        CreateAccountCommand command,
        CancellationToken cancellationToken)
    {
        var account = new Account
        {
            Name      = command.Name,
            Type      = command.Type,
            Balance   = command.Balance,
            Color     = command.Color,
            Icon      = command.Icon,
            UserId    = command.UserId,
            CreatedAt = DateTime.UtcNow,
        };

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync(cancellationToken);
        return account.Id;
    }
}
