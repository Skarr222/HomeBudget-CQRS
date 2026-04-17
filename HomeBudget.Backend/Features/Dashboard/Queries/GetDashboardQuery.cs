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
    private readonly HomeBudgetDbContext _db;

    public GetDashboardQueryHandler(HomeBudgetDbContext db) => _db = db;

    public async Task<DashboardDto> Handle(GetDashboardQuery r, CancellationToken ct)
    {
        var month = r.Month ?? DateTime.UtcNow.Month;
        var year = r.Year ?? DateTime.UtcNow.Year;

        var transactions = await _db
            .Transactions.Include(t => t.User)
            .Include(t => t.Category)
            .Include(t => t.Account)
            .Include(t => t.Receipt)
            .Include(t => t.Splits)
            .Include(t => t.TransactionTags)
            .ThenInclude(tt => tt.Tag)
            .Where(t =>
                t.HouseholdId == r.HouseholdId && t.Date.Month == month && t.Date.Year == year
            )
            .ToListAsync(ct);

        var income = transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
        var expenses = transactions
            .Where(t => t.Type == TransactionType.Expense)
            .Sum(t => t.Amount);

        var topCats = transactions
            .Where(t => t.Type == TransactionType.Expense)
            .GroupBy(t => new
            {
                t.Category.Name,
                t.Category.Color,
                t.Category.Icon,
            })
            .Select(g => new CategorySpendingDto(
                g.Key.Name,
                g.Key.Color,
                g.Key.Icon,
                g.Sum(t => t.Amount),
                expenses > 0 ? (double)(g.Sum(t => t.Amount) / expenses * 100) : 0,
                g.Count()
            ))
            .OrderByDescending(c => c.Amount)
            .Take(5)
            .ToList();

        var recent = transactions
            .OrderByDescending(t => t.Date)
            .Take(10)
            .Select(t => new TransactionDto(
                t.Id,
                t.Title,
                t.Amount,
                t.Date,
                t.Note,
                t.Type,
                t.PaymentMethod,
                t.IsShared,
                t.User.FirstName + " " + t.User.LastName,
                t.UserId,
                t.Category.Name,
                t.Category.Icon,
                t.Category.Color,
                t.CategoryId,
                t.Account.Name,
                t.AccountId,
                t.TransactionTags.Select(tt => tt.Tag.Name).ToList(),
                t.Receipt != null,
                t.Splits.Any(),
                t.CreatedAt
            ))
            .ToList();

        var budgets = await _db
            .Budgets.Include(b => b.User)
            .Include(b => b.Category)
            .Where(b => b.HouseholdId == r.HouseholdId && b.Month == month && b.Year == year)
            .ToListAsync(ct);

        var budgetDtos = budgets
            .Select(b =>
            {
                var spent = transactions
                    .Where(t => t.CategoryId == b.CategoryId && t.Type == TransactionType.Expense)
                    .Sum(t => t.Amount);
                return new BudgetDto(
                    b.Id,
                    b.Amount,
                    spent,
                    b.Amount - spent,
                    b.Amount > 0 ? (double)(spent / b.Amount * 100) : 0,
                    b.Month,
                    b.Year,
                    b.Category.Name,
                    b.Category.Icon,
                    b.Category.Color,
                    b.CategoryId,
                    b.User.FirstName + " " + b.User.LastName,
                    b.UserId
                );
            })
            .ToList();

        var goals = await _db
            .SavingsGoals.Where(sg => sg.HouseholdId == r.HouseholdId && !sg.IsCompleted)
            .Select(sg => new SavingsGoalDto(
                sg.Id,
                sg.Name,
                sg.TargetAmount,
                sg.CurrentAmount,
                sg.TargetAmount > 0 ? (double)(sg.CurrentAmount / sg.TargetAmount * 100) : 0,
                sg.Deadline,
                sg.Icon,
                sg.Color,
                sg.IsCompleted
            ))
            .ToListAsync(ct);

        var upcomingBills = await _db
            .BillPayments.Include(bp => bp.Bill)
            .Where(bp => bp.Bill.HouseholdId == r.HouseholdId && bp.Status != BillStatus.Paid)
            .OrderBy(bp => bp.DueDate)
            .Take(5)
            .Select(bp => new BillPaymentDto(
                bp.Id,
                bp.BillId,
                bp.Bill.Name,
                bp.Amount,
                bp.DueDate,
                bp.PaidDate,
                bp.PaymentMethod,
                bp.Status,
                bp.TransactionId
            ))
            .ToListAsync(ct);

        var accounts = await _db
            .Accounts.Include(a => a.User)
            .Where(a =>
                _db.HouseholdMembers.Any(hm =>
                    hm.HouseholdId == r.HouseholdId && hm.UserId == a.UserId
                )
            )
            .Select(a => new AccountDto(
                a.Id,
                a.Name,
                a.Type,
                a.Balance,
                a.Color,
                a.Icon,
                a.UserId,
                a.User.FirstName + " " + a.User.LastName
            ))
            .ToListAsync(ct);

        return new DashboardDto(
            income,
            expenses,
            income - expenses,
            topCats,
            recent,
            budgetDtos,
            goals,
            upcomingBills,
            accounts
        );
    }
}
