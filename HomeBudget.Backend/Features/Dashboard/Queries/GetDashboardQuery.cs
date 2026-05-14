using HomeBudget.Application.Accounts.Maps;
using HomeBudget.Application.Bills.Maps;
using HomeBudget.Application.Budgets;
using HomeBudget.Application.Budgets.Maps;
using HomeBudget.Application.Dashboard;
using HomeBudget.Application.SavingsGoals.Maps;
using HomeBudget.Application.Transactions.Maps;
using HomeBudget.Data.Context;
using HomeBudget.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Application.Dashboard.Queries;

public record GetDashboardQuery(int HouseholdId, int? Month, int? Year) : IRequest<DashboardDto>;

public class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, DashboardDto>
{
    private readonly HomeBudgetDbContext _context;

    public GetDashboardQueryHandler(HomeBudgetDbContext context) => _context = context;

    public async Task<DashboardDto> Handle(
        GetDashboardQuery query,
        CancellationToken cancellationToken
    )
    {
        var month = query.Month ?? DateTime.UtcNow.Month;
        var year = query.Year ?? DateTime.UtcNow.Year;

        var transactions = await _context
            .Transactions.Include(transaction => transaction.User)
            .Include(transaction => transaction.Category)
            .Include(transaction => transaction.Account)
            .Include(transaction => transaction.Receipt)
            .Include(transaction => transaction.Splits)
            .Include(transaction => transaction.TransactionTags)
            .ThenInclude(transactionTag => transactionTag.Tag)
            .Where(transaction =>
                transaction.HouseholdId == query.HouseholdId
                && transaction.Date.Month == month
                && transaction.Date.Year == year
            )
            .ToListAsync(cancellationToken);

        var totalIncome = transactions
            .Where(transaction => transaction.Type == TransactionType.Income)
            .Sum(transaction => transaction.Amount);

        var totalExpenses = transactions
            .Where(transaction => transaction.Type == TransactionType.Expense)
            .Sum(transaction => transaction.Amount);

        var topCategories = transactions
            .Where(transaction => transaction.Type == TransactionType.Expense)
            .GroupBy(transaction => new
            {
                transaction.Category.Name,
                transaction.Category.Color,
                transaction.Category.Icon,
            })
            .Select(group => new CategorySpendingDto(
                group.Key.Name,
                group.Key.Color,
                group.Key.Icon,
                group.Sum(transaction => transaction.Amount),
                totalExpenses > 0
                    ? (double)(group.Sum(transaction => transaction.Amount) / totalExpenses * 100)
                    : 0,
                group.Count()
            ))
            .OrderByDescending(category => category.Amount)
            .Take(5)
            .ToList();

        var recentTransactions = transactions
            .OrderByDescending(transaction => transaction.Date)
            .Take(10)
            .Select(TransactionMappings.ToDto)
            .ToList();

        var budgets = await _context
            .Budgets.Include(budget => budget.User)
            .Include(budget => budget.Category)
            .Where(budget =>
                budget.HouseholdId == query.HouseholdId
                && budget.Month == month
                && budget.Year == year
            )
            .ToListAsync(cancellationToken);

        var activeBudgets = budgets
            .Select(budget =>
            {
                var spent = transactions
                    .Where(transaction =>
                        transaction.CategoryId == budget.CategoryId
                        && transaction.Type == TransactionType.Expense
                    )
                    .Sum(transaction => transaction.Amount);

                return BudgetMappings.ToDto(budget, spent);
            })
            .ToList();

        var savingsGoals = (
            await _context
                .SavingsGoals.Where(goal =>
                    goal.HouseholdId == query.HouseholdId && !goal.IsCompleted
                )
                .ToListAsync(cancellationToken)
        )
            .Select(SavingsGoalMappings.ToDto)
            .ToList();

        var upcomingBills = (
            await _context
                .BillPayments.Include(payment => payment.Bill)
                .Where(payment =>
                    payment.Bill.HouseholdId == query.HouseholdId
                    && payment.Status != BillStatus.Paid
                )
                .OrderBy(payment => payment.DueDate)
                .Take(5)
                .ToListAsync(cancellationToken)
        )
            .Select(payment => BillPaymentMappings.ToDto(payment, payment.Bill.Name))
            .ToList();

        var accounts = (
            await _context
                .Accounts.Include(account => account.User)
                .Where(account =>
                    _context.HouseholdMembers.Any(member =>
                        member.HouseholdId == query.HouseholdId && member.UserId == account.UserId
                    )
                )
                .ToListAsync(cancellationToken)
        )
            .Select(AccountMappings.ToDto)
            .ToList();

        return new DashboardDto(
            totalIncome,
            totalExpenses,
            totalIncome - totalExpenses,
            topCategories,
            recentTransactions,
            activeBudgets,
            savingsGoals,
            upcomingBills,
            accounts
        );
    }
}
