using Microsoft.AspNetCore.Mvc;
using MediatR;
using OnlineShopping.Shared.Application.Filters;

namespace OnlineShopping.CatalogService.API.Controllers;

[ApiController]
[ApiExceptionFilter]
[Route("api/[controller]")]
public class CatalogControllerBase : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    [NonAction]
    protected new CreatedAtActionResult CreatedAtAction(string? actionName, object? routeValues)
        => CreatedAtAction(actionName, controllerName: null, routeValues: routeValues, value: null);
}
