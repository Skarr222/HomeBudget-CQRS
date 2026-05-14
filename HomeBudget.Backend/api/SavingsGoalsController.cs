using HomeBudget.Application.SavingsGoals;
using HomeBudget.Application.SavingsGoals.Commands;
using HomeBudget.Application.SavingsGoals.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HomeBudget.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SavingsGoalsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SavingsGoalsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<SavingsGoalDto>>> GetAll([FromQuery] int householdId) =>
        Ok(await _mediator.Send(new GetAllSavingsGoalsQuery(householdId)));

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateSavingsGoalCommand command)
    {
        var createdId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { }, createdId);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSavingsGoalCommand command)
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
        var success = await _mediator.Send(new DeleteSavingsGoalCommand(id));
        return success ? NoContent() : NotFound();
    }
}
