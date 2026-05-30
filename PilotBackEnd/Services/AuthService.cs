
public interface IAuthService
{
    Task<dynamic?> LoginAsync(LoginRequest request);
    Task<dynamic?> RegisterAsync(RegisterRequest request);
}

public class AuthService : IAuthService
{
    private readonly IAuthRepository _repo;

    public AuthService(IAuthRepository repo)
    {
        _repo = repo;
    }

    public async Task<dynamic?> LoginAsync(LoginRequest request)
    {
        return await _repo.LoginAsync(request);
    }

    public async Task<dynamic?> RegisterAsync(RegisterRequest request)
    {
        return await _repo.RegisterAsync(request);
    }
}