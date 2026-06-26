using Microsoft.AspNetCore.Mvc;

namespace OrderTrack.Api.Controllers;

[ApiController]
[Route("api/auth-test")]
public class AuthTestController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthTestController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginTestDto dto)
    {
        var projectUrl = _configuration["Supabase:ProjectUrl"];
        var anonKey = _configuration["Supabase:AnonKey"];

        using var client = new HttpClient();

        var response = await client.PostAsJsonAsync(
            $"{projectUrl}/auth/v1/token?grant_type=password",
            new
            {
                email = dto.Email,
                password = dto.Password
            });

        var content = await response.Content.ReadAsStringAsync();

        return Content(content, "application/json");
    }
}

public class LoginTestDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}