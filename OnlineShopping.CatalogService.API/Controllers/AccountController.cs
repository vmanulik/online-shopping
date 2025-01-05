using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace OnlineShopping.CatalogService.API.Controllers
{
    public class AccountController : CatalogControllerBase
    {
        //[HttpGet]
        //[ApiExplorerSettings(IgnoreApi = true)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //public IActionResult AccessDenied()
        //{
        //    return StatusCode((int)HttpStatusCode.Forbidden);
        //}

        //[HttpGet("logout")]
        //public async Task UnAuthorize()
        //{
        //    if (User.Identity != null && User.Identity.IsAuthenticated)
        //    {
        //        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //        await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        //        return;
        //    }
        //}

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> LoginCallback()
        {
            var authResult = await HttpContext.AuthenticateAsync(OpenIdConnectDefaults.AuthenticationScheme);
            if (authResult?.Succeeded != true)
            {
                return RedirectToAction("Login");
            }

            var accessToken = authResult!.Properties!.GetTokenValue("access_token");
            var refreshToken = authResult!.Properties!.GetTokenValue("refresh_token");

            HttpContext.Session.SetString("access_token", accessToken!);
            HttpContext.Session.SetString("refresh_token", refreshToken!);

            return RedirectToAction("GetCategories", "Category");
        }
    }
}