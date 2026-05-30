using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (request == null)
        {
            return BadRequest(new
            {
                success = false,
                message = "Missing credentials"
            });
        }

        var user = await _service.LoginAsync(request);

        if (user == null)
        {
            return Unauthorized(new
            {
                success = false,
                message = "We couldn't log you in. Check your credentials and try again."
            });
        }

        return Ok(new
        {
            success = true,
            firstname = user.firstname,
            email = user.email,
            iduser = user.iduser,
            username = user.username
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (request == null ||
            string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Password) ||
            string.IsNullOrWhiteSpace(request.Firstname) ||
            string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest(new
            {
                success = false,
                message = "All fields are required"
            });
        }

        var result = await _service.RegisterAsync(request);

        if (result == null)
        {
            return Conflict(new
            {
                success = false,
                message = "Username already exists"
            });
        }

        return Ok(new
        {
            success = true,
            user = result
        });
    }
}

// DTO

public class LoginRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}

public class RegisterRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Firstname { get; set; }
    public string? Email { get; set; }
}