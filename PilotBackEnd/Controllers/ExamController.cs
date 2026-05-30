using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/pilot-exam")]
public class PilotExamController : ControllerBase
{
    private readonly IPilotExamService _service;
    private readonly ILogger<PilotExamController> _logger;

    public PilotExamController(IPilotExamService service, ILogger<PilotExamController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [Authorize]
    [HttpPost("select_pilot_exam")]
    public async Task<IActionResult> SelectPilotExam([FromBody] IdUserRequest request)
    {
        try
        {
            _logger.LogInformation("POST /api/pilot-exam/select_pilot_exam called");

            if (request == null || request.IdUser <= 0)
            {
                return BadRequest(new { error = "Missing iduser" });
            }

            var exams = await _service.GetUserExamsAsync(request.IdUser);

            if (exams == null)
            {
                return NotFound(new { error = "Exam not found" });
            }

            var examList = exams.ToList();

            _logger.LogInformation("Exams retrieved: {Exams}", examList.Count());

            return Ok(new { success = true, exams = exams });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in SelectPilotExam");

            return StatusCode(500, new
            {
                error = "Internal server error."
            }); 
        }
    }

    [Authorize]
    [HttpPost("select_one_exam")]
    public async Task<IActionResult> SelectOneExam([FromBody] IdExamRequest request)
    {
        try
        {
            _logger.LogInformation("POST /api/pilot-exam/select_one_exam called");

            if (request == null || request.IdExam <= 0)
            {
                return BadRequest(new { error = "Missing idexam" });
            }

            var exam = await _service.GetOneExamAsync(request.IdExam);

            if (exam == null)
            {
                return NotFound(new { error = "Exam not found" });
            }

            _logger.LogInformation("Exam {ExamId} retrieved", request.IdExam);

            return Ok(new { success = true, exam = exam });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting exam {ExamId}", request.IdExam);

            return StatusCode(500, new
            {
                error = "Internal server error."
            });
        }
    }

    [Authorize]
    [HttpPost("insert_pilot_exam")]
    public async Task<IActionResult> Insert([FromBody] InsertPilotExamRequest request)
    {
        try
        {
            _logger.LogInformation("POST /api/pilot-exam/insert_pilot_exam");

            if (request == null)
            {
                return BadRequest(new { error = "Request is null" });
            }
                
            if (request.IdUser <= 0)
            {
                return BadRequest(new { error = "Invalid IdUser" });
            }
                
            var createdExam = await _service.InsertAsync(request);

            _logger.LogInformation("Exam {IdExam} created successfully", createdExam.IdExam);

            return Created($"/api/pilot-exam/{createdExam.IdExam}", new
            {
                message = "Exam created successfully",
                exam = createdExam
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while inserting exam.");

            return StatusCode(500, new
            {
                error = "Internal server error."
            });
        }
    }

    [Authorize]
    [HttpPost("delete_pilot_exam")]
    public async Task<IActionResult> Delete([FromBody] IdExamRequest request)
    {
        try
        {
            _logger.LogInformation("POST /api/pilot-exam/delete_pilot_exam");

            if (request == null)
            {
                return BadRequest(new { error = "Request is null" });
            }

            if (request.IdExam <= 0)
            {
                return BadRequest(new { error = "Invalid IdExam" });
            }

            var result = await _service.DeleteAsync(request.IdExam);

            if (result == null)
            {
                return NotFound(new { success = false, error = "Exam not found" });
            }
                
            _logger.LogInformation("Exam {IdExam} deleted successfully", request.IdExam);

            return Ok(new
            {
                success = true,
                deletedExam = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting exam {IdExam}", request.IdExam);

            return StatusCode(500, new
            {
                error = "Internal server error."
            });
        }
    }

    [Authorize]
    [HttpPost("clear_all_exams")]
    public async Task<IActionResult> ClearAll([FromBody] IdUserRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { error = "Request is null" });
            }

            if (request.IdUser <= 0)
            {
                return BadRequest(new { error = "Invalid IdUser" });
            }

            var deletedCount = await _service.ClearAllAsync(request.IdUser);

            _logger.LogInformation("All exams from user {IdUser} deleted successfully", request.IdUser);

            return Ok(new
            {
                success = true,
                deletedCount
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting all exams from user {IdUser}", request.IdUser);

            return StatusCode(500, new
            {
                error = "Internal server error."
            });
        }      
    }
}

// DTOs

public class IdUserRequest
{
    public int IdUser { get; set; }
}

public class IdExamRequest
{
    public int IdExam { get; set; }
}

public class InsertPilotExamRequest
{
    public int IdUser { get; set; }
    public string? Mode { get; set; }
    public string? Title { get; set; }
    public int NbQuestions { get; set; }
    public string? Duration { get; set; }
    public string? ExamDate { get; set; }
    public int Score { get; set; }
    public string? AnswerString { get; set; }
}

public class PilotExam
{
    public long IdExam { get; set; }
    public long IdUser { get; set; }
    public string? Mode { get; set; }
    public string? Title { get; set; }
    public int NbQuestions { get; set; }
    public string? Duration { get; set; }
    public string? ExamDate { get; set; }
    public int Score { get; set; }
    public string? AnswerString { get; set; }
}
