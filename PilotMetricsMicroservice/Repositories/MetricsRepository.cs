using Npgsql;

public interface IMetricsRepository
{
    Task<long> GetUsersCountAsync();
    Task<long> GetExamsCountAsync();
}

public class MetricsRepository : IMetricsRepository
{
    private readonly IConfiguration _config;
    private readonly ILogger<MetricsRepository> _logger;

    public MetricsRepository(IConfiguration config, ILogger<MetricsRepository> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task<long> GetUsersCountAsync()
    {
        var start = DateTime.UtcNow;

        try
        {
            await using var conn = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection"));

            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM pilot_user", conn);

            var result = await cmd.ExecuteScalarAsync();

            return Convert.ToInt64(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while counting users in database");
            throw;
        }
        finally
        {
            _logger.LogInformation("GetUsersCountAsync executed in {Time} ms", (DateTime.UtcNow - start).TotalMilliseconds);
        }
    }

    public async Task<long> GetExamsCountAsync()
    {
        var start = DateTime.UtcNow;

        try
        {
            await using var conn = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection"));

            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM pilot_exam", conn);

            var result = await cmd.ExecuteScalarAsync();

            return Convert.ToInt64(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while counting exams in database");
            throw;
        }
        finally
        {
            _logger.LogInformation("GetExamsCountAsync executed in {Time} ms", (DateTime.UtcNow - start).TotalMilliseconds);
        }
    }
}