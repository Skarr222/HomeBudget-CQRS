using HomeBudget.Application.Transactions;
using HomeBudget.Application.Transactions.Commands;
using HomeBudget.Application.Transactions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HomeBudget.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator) => _mediator = mediator;

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
            await _mediator.Send(
                new GetAllTransactionsQuery(householdId, userId, categoryId, accountId, month, year)
            )
        );

    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionDto>> GetById(int id)
    {
        var transaction = await _mediator.Send(new GetTransactionByIdQuery(id));
        return transaction is null ? NotFound() : Ok(transaction);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateTransactionCommand command)
    {
        var createdId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = createdId }, createdId);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTransactionCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        var success = await _mediator.Send(command);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _mediator.Send(new DeleteTransactionCommand(id));
        return success ? NoContent() : NotFound();
    }
}
