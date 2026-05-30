public interface IPilotExamService
{
    Task<List<dynamic>> GetUserExamsAsync(int idUser);
    Task<dynamic> GetOneExamAsync(int idExam);
    Task<PilotExam> InsertAsync(InsertPilotExamRequest request);
    Task<object?> DeleteAsync(int idExam);
    Task<int> ClearAllAsync(int idUser);
}

public class PilotExamService : IPilotExamService
{
    private readonly IPilotExamRepository _repo;
    private readonly ILogger<PilotExamService> _logger;

    public PilotExamService(IPilotExamRepository repo, ILogger<PilotExamService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<List<dynamic>> GetUserExamsAsync(int idUser)
    {
        return await _repo.GetByUserIdAsync(idUser);
    }

    public async Task<dynamic> GetOneExamAsync(int idExam)
    {
        return await _repo.GetByExamIdAsync(idExam);
    }

    public async Task<PilotExam> InsertAsync(InsertPilotExamRequest request)
    {
        return await _repo.InsertAsync(request);
    }

    public async Task<object?> DeleteAsync(int idExam)
    {
        return await _repo.DeleteAsync(idExam);
    }

    public async Task<int> ClearAllAsync(int idUser)
    {
        return await _repo.ClearAllAsync(idUser);
    }
}