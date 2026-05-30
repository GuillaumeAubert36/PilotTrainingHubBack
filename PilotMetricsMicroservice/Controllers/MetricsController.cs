using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/metrics")]
public class MetricsController : ControllerBase
{
    private readonly IMetricsService _service;
    private readonly ILogger<MetricsController> _logger;

    public MetricsController(IMetricsService service, ILogger<MetricsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Get()
    {       
        try
        {
            _logger.LogInformation("GET /api/metrics called");

            var result = await _service.GetMetricsAsync();

            if (result == null)
            {
                return NotFound(new
                {
                    message = "No metrics found."
                });
            }

            _logger.LogInformation("Metrics retrieved: Users={Users}, Exams={Exams}",result.TotalUsers, result.TotalExams);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                error = "Internal server error.",
                details = ex.Message
            });
        }

    }
}

// DTOs

public class MetricsDto
{
    public long TotalUsers { get; set; }
    public long TotalExams { get; set; }
}