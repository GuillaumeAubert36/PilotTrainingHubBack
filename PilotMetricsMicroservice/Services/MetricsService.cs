public interface IMetricsService
{
    Task<MetricsDto> GetMetricsAsync();
}

public class MetricsService : IMetricsService
{
    private readonly IMetricsRepository _repo;
    private readonly ILogger<MetricsService> _logger;

    public MetricsService(IMetricsRepository repo, ILogger<MetricsService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<MetricsDto> GetMetricsAsync()
    {
        var users = await _repo.GetUsersCountAsync();
        _logger.LogInformation("Users count retrieved: {Users}", users);

        var exams = await _repo.GetExamsCountAsync();
        _logger.LogInformation("Exams count retrieved: {Exams}", exams);

        return new MetricsDto
        {
            TotalUsers = users,
            TotalExams = exams
        };
    }
}