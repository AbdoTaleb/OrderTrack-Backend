using Microsoft.AspNetCore.Mvc;

namespace OrderTrack.Api.Controllers;

[ApiController]
[Route("api/debug-auth")]
public class DebugAuthController : ControllerBase
{
    [HttpGet]
    public IActionResult Check()
    {
        var authorization = Request.Headers.Authorization.ToString();

        var token = authorization.StartsWith("Bearer ")
            ? authorization["Bearer ".Length..]
            : authorization;

        return Ok(new
        {
            rawAuthorizationStartsWith = authorization.Length > 30 ? authorization[..30] : authorization,
            authorizationLength = authorization.Length,
            tokenStartsWith = token.Length > 30 ? token[..30] : token,
            tokenLength = token.Length,
            partsCount = token.Split('.').Length,
            firstPart = token.Split('.').FirstOrDefault()
        });
    }
}