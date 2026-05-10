using HomeBudget.Application.Accounts;
using HomeBudget.Application.BillPayments;
using HomeBudget.Application.Budgets;
using HomeBudget.Application.SavingsGoals;
using HomeBudget.Application.Transactions;
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
            .Where(t => t.Type == TransactionType.Income)
            .Sum(t => t.Amount);
        var totalExpenses = transactions
            .Where(t => t.Type == TransactionType.Expense)
            .Sum(t => t.Amount);

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
            .Select(transaction => new TransactionDto(
                transaction.Id,
                transaction.Title,
                transaction.Amount,
                transaction.Date,
                transaction.Note,
                transaction.Type,
                transaction.PaymentMethod,
                transaction.IsShared,
                transaction.User.FirstName + " " + transaction.User.LastName,
                transaction.UserId,
                transaction.Category.Name,
                transaction.Category.Icon,
                transaction.Category.Color,
                transaction.CategoryId,
                transaction.Account.Name,
                transaction.AccountId,
                transaction.TransactionTags.Select(tt => tt.Tag.Name).ToList(),
                transaction.Receipt != null,
                transaction.Splits.Any(),
                transaction.CreatedAt
            ))
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

                return new BudgetDto(
                    budget.Id,
                    budget.Amount,
                    spent,
                    budget.Amount - spent,
                    budget.Amount > 0 ? (double)(spent / budget.Amount * 100) : 0,
                    budget.Month,
                    budget.Year,
                    budget.Category.Name,
                    budget.Category.Icon,
                    budget.Category.Color,
                    budget.CategoryId,
                    budget.User.FirstName + " " + budget.User.LastName,
                    budget.UserId
                );
            })
            .ToList();

        var savingsGoals = await _context
            .SavingsGoals.Where(goal => goal.HouseholdId == query.HouseholdId && !goal.IsCompleted)
            .Select(goal => new SavingsGoalDto(
                goal.Id,
                goal.Name,
                goal.TargetAmount,
                goal.CurrentAmount,
                goal.TargetAmount > 0 ? (double)(goal.CurrentAmount / goal.TargetAmount * 100) : 0,
                goal.Deadline,
                goal.Icon,
                goal.Color,
                goal.IsCompleted
            ))
            .ToListAsync(cancellationToken);

        var upcomingBills = await _context
            .BillPayments.Include(payment => payment.Bill)
            .Where(payment =>
                payment.Bill.HouseholdId == query.HouseholdId && payment.Status != BillStatus.Paid
            )
            .OrderBy(payment => payment.DueDate)
            .Take(5)
            .Select(payment => new BillPaymentDto(
                payment.Id,
                payment.BillId,
                payment.Bill.Name,
                payment.Amount,
                payment.DueDate,
                payment.PaidDate,
                payment.PaymentMethod,
                payment.Status,
                payment.TransactionId
            ))
            .ToListAsync(cancellationToken);

        var accounts = await _context
            .Accounts.Include(account => account.User)
            .Where(account =>
                _context.HouseholdMembers.Any(member =>
                    member.HouseholdId == query.HouseholdId && member.UserId == account.UserId
                )
            )
            .Select(account => new AccountDto(
                account.Id,
                account.Name,
                account.Type,
                account.Balance,
                account.Color,
                account.Icon,
                account.UserId,
                account.User.FirstName + " " + account.User.LastName
            ))
            .ToListAsync(cancellationToken);

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
