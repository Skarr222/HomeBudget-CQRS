using HomeBudget.Application.Bills;
using HomeBudget.Application.Bills.Commands;
using HomeBudget.Application.Bills.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HomeBudget.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BillsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BillsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<BillDto>>> GetAll([FromQuery] int householdId) =>
        Ok(await _mediator.Send(new GetAllBillsQuery(householdId)));

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateBillCommand command)
    {
        var createdId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { }, createdId);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBillCommand command)
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
        var success = await _mediator.Send(new DeleteBillCommand(id));
        return success ? NoContent() : NotFound();
    }

    [HttpPost("{id}/pay")]
    public async Task<ActionResult<int>> Pay(int id, [FromBody] PayBillCommand command)
    {
        if (id != command.BillId)
        {
            return BadRequest();
        }

        var transactionId = await _mediator.Send(command);
        return Ok(transactionId);
    }
}
