using HomeBudget.Application.Categories;
using HomeBudget.Application.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HomeBudget.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetAll() =>
        Ok(await _mediator.Send(new GetAllCategoriesQuery()));
}
