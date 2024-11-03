using Microsoft.AspNetCore.Mvc;
using MediatR;
using OnlineShopping.Shared.Application.Filters;

namespace OnlineShopping.CatalogService.API;

[ApiController]
[ApiExceptionFilter]
[Route("[controller]")]
public class CartControllerBase : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
