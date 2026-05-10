using HomeBudget.Application.ShoppingItems;
using HomeBudget.Application.ShoppingItems.Commands;
using HomeBudget.Application.ShoppingItems.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HomeBudget.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShoppingItemsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShoppingItemsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<ShoppingItemDto>>> GetAll([FromQuery] int shoppingListId) =>
        Ok(await _mediator.Send(new GetShoppingItemsQuery(shoppingListId)));

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateShoppingItemCommand command)
    {
        var createdId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { }, createdId);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateShoppingItemCommand command)
    {
        if (id != command.Id) return BadRequest();
        var success = await _mediator.Send(command);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _mediator.Send(new DeleteShoppingItemCommand(id));
        return success ? NoContent() : NotFound();
    }
}
