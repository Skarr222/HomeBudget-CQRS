using HomeBudget.Application.Dashboard;
using HomeBudget.Application.Dashboard.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HomeBudget.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IMediator _m;

    public DashboardController(IMediator m) => _m = m;

    [HttpGet("{householdId}")]
    public async Task<ActionResult<DashboardDto>> Get(
        int householdId,
        [FromQuery] int? month,
        [FromQuery] int? year
    ) => Ok(await _m.Send(new GetDashboardQuery(householdId, month, year)));
}
