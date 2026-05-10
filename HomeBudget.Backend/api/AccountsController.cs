using HomeBudget.Application.Accounts;
using HomeBudget.Application.Accounts.Commands;
using HomeBudget.Application.Accounts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HomeBudget.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<AccountDto>>> GetAll(
        [FromQuery] int? userId,
        [FromQuery] int? householdId
    ) => Ok(await _mediator.Send(new GetAllAccountsQuery(userId, householdId)));

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateAccountCommand command)
    {
        var createdId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { }, createdId);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAccountCommand command)
    {
        if (id != command.Id) return BadRequest();
        var success = await _mediator.Send(command);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _mediator.Send(new DeleteAccountCommand(id));
        return success ? NoContent() : NotFound();
    }
}
