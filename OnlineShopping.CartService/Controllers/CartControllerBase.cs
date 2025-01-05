using Microsoft.AspNetCore.Mvc;
using MediatR;
using OnlineShopping.Shared.Application.Filters;

namespace OnlineShopping.CartService.API;

[ApiController]
[ApiExceptionFilter]
[Route("api/[controller]")]
public class CartControllerBase : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
