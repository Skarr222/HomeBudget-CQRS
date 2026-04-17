using HomeBudget.Application.Transactions;
using HomeBudget.Application.Transactions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HomeBudget.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _m;

    public TransactionsController(IMediator m) => _m = m;

    [HttpGet]
    public async Task<ActionResult<List<TransactionDto>>> GetAll(
        [FromQuery] int? householdId,
        [FromQuery] int? userId,
        [FromQuery] int? categoryId,
        [FromQuery] int? accountId,
        [FromQuery] int? month,
        [FromQuery] int? year
    ) =>
        Ok(
            await _m.Send(
                new GetAllTransactionsQuery(householdId, userId, categoryId, accountId, month, year)
            )
        );

    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionDto>> GetById(int id)
    {
        var r = await _m.Send(new GetTransactionByIdQuery(id));
        return r is null ? NotFound() : Ok(r);
    }
}
