using HomeBudget.Application.ShoppingLists;
using HomeBudget.Application.ShoppingLists.Commands;
using HomeBudget.Application.ShoppingLists.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HomeBudget.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShoppingListsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShoppingListsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<ShoppingListDto>>> GetAll([FromQuery] int householdId) =>
        Ok(await _mediator.Send(new GetAllShoppingListsQuery(householdId)));

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateShoppingListCommand command)
    {
        var createdId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { }, createdId);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateShoppingListCommand command)
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
        var success = await _mediator.Send(new DeleteShoppingListCommand(id));
        return success ? NoContent() : NotFound();
    }
}
