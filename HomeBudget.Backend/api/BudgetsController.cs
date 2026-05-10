using HomeBudget.Application.Budgets;
using HomeBudget.Application.Budgets.Commands;
using HomeBudget.Application.Budgets.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HomeBudget.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BudgetsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BudgetsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<BudgetDto>>> GetAll(
        [FromQuery] int householdId,
        [FromQuery] int? month,
        [FromQuery] int? year
    ) => Ok(await _mediator.Send(new GetAllBudgetsQuery(householdId, month, year)));

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateBudgetCommand command)
    {
        var createdId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { }, createdId);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBudgetCommand command)
    {
        if (id != command.Id) return BadRequest();
        var success = await _mediator.Send(command);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _mediator.Send(new DeleteBudgetCommand(id));
        return success ? NoContent() : NotFound();
    }
}
