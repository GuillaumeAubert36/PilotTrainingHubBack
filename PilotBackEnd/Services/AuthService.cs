using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public interface IAuthService
{
    Task<dynamic?> LoginAsync(LoginRequest request);
    Task<dynamic?> RegisterAsync(RegisterRequest request);
    string GenerateToken(dynamic user);
}

public class AuthService : IAuthService
{
    private readonly IAuthRepository _repo;
    private readonly IConfiguration _config;

    public AuthService(IAuthRepository repo, IConfiguration config)
    {
        _repo = repo;
        _config = config;
    }

    public async Task<dynamic?> LoginAsync(LoginRequest request)
    {
        return await _repo.LoginAsync(request);
    }

    public async Task<dynamic?> RegisterAsync(RegisterRequest request)
    {
        return await _repo.RegisterAsync(request);
    }

    public string GenerateToken(dynamic user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.iduser.ToString()),
            new Claim(ClaimTypes.Name, user.username),
            new Claim("email", user.email),
            new Claim(ClaimTypes.Role, user.role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

        var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                int.Parse(_config["Jwt:ExpirationMinutes"] ?? "60")),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}