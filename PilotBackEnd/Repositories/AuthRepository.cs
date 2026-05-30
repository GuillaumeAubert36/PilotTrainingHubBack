using Npgsql;

public interface IAuthRepository
{
    Task<dynamic?> LoginAsync(LoginRequest request);
    Task<dynamic?> RegisterAsync(RegisterRequest request);
}

public class AuthRepository : IAuthRepository
{
    private readonly string? _connectionString;

    public AuthRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection");
    }

    public async Task<dynamic?> LoginAsync(LoginRequest request)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var cmd = new NpgsqlCommand(@"
            SELECT iduser, username, firstname, email
            FROM pilot_user
            WHERE LOWER(TRIM(username)) = LOWER(TRIM(@username))
            AND TRIM(password) = TRIM(@password)
            LIMIT 1;
        ", conn);

        cmd.Parameters.AddWithValue("@username", (object?)request.Username ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@password", (object?)request.Password ?? DBNull.Value);

        await using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new
            {
                iduser = reader["iduser"],
                username = reader["username"],
                firstname = reader["firstname"],
                email = reader["email"]
            };
        }

        return null;
    }

    public async Task<dynamic?> RegisterAsync(RegisterRequest request)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        // check username
        var checkCmd = new NpgsqlCommand(@"
            SELECT 1 FROM pilot_user
            WHERE username = @username
            LIMIT 1;
        ", conn);

        checkCmd.Parameters.AddWithValue("@username", (object?)request.Username ?? DBNull.Value);

        var exists = await checkCmd.ExecuteScalarAsync();
        if (exists != null)
            return null;

        // insert
        var cmd = new NpgsqlCommand(@"
            INSERT INTO pilot_user (username, password, firstname, email)
            VALUES (@username, @password, @firstname, @email)
            RETURNING iduser, username, firstname, email;
        ", conn);

        cmd.Parameters.AddWithValue("@username", (object?)request.Username ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@password", (object?)request.Password ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@firstname", (object?)request.Firstname ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@email", (object?)request.Email ?? DBNull.Value);

        await using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new
            {
                iduser = reader["iduser"],
                username = reader["username"],
                firstname = reader["firstname"],
                email = reader["email"]
            };
        }

        return null;
    }

}