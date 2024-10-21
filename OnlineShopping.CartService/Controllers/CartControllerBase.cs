using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace OnlineShopping.CatalogService.API;

[ApiController]
[Route("[controller]")]
public class CartControllerBase : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
