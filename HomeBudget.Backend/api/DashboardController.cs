using HomeBudget.Application.Dashboard;
using HomeBudget.Application.Dashboard.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HomeBudget.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{householdId}")]
    public async Task<ActionResult<DashboardDto>> Get(
        int householdId,
        [FromQuery] int? month,
        [FromQuery] int? year
    ) => Ok(await _mediator.Send(new GetDashboardQuery(householdId, month, year)));
}
