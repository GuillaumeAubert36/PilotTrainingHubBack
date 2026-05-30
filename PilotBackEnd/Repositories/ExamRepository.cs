using Npgsql;

public interface IPilotExamRepository
{
    Task<List<dynamic>> GetByUserIdAsync(int idUser);
    Task<dynamic> GetByExamIdAsync(int idExam);
    Task<PilotExam> InsertAsync(InsertPilotExamRequest r);
    Task<object?> DeleteAsync(int idExam);
    Task<int> ClearAllAsync(int idUser);
}

public class PilotExamRepository : IPilotExamRepository
{
    private readonly string? _connectionString;
    private readonly ILogger<PilotExamRepository> _logger;

    public PilotExamRepository(IConfiguration config, ILogger<PilotExamRepository> logger)
    {
        _connectionString = config.GetConnectionString("DefaultConnection");
        _logger = logger;
    }

    public async Task<List<dynamic>> GetByUserIdAsync(int idUser)
    {
        var list = new List<dynamic>();

        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var cmd = new NpgsqlCommand(@"
            SELECT *
            FROM pilot_exam
            WHERE iduser = @iduser
            ORDER BY idexam DESC", conn);

        cmd.Parameters.AddWithValue("@iduser", idUser);

        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(new
            {
                idexam = reader["idexam"],
                iduser = reader["iduser"],
                mode = reader["mode"],
                title = reader["title"],
                nb_questions = reader["nb_questions"],
                duration = reader["duration"],
                exam_date = reader["exam_date"],
                score = reader["score"],
                answer_string = reader["answer_string"]
            });
        }

        return list;
    }

    public async Task<dynamic> GetByExamIdAsync(int idExam)
    {
        var list = new List<dynamic>();

        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var cmd = new NpgsqlCommand(@"
            SELECT *
            FROM pilot_exam
            WHERE idexam = @idexam", conn);

        cmd.Parameters.AddWithValue("@idexam", idExam);

        await using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            list.Add(new
            {
                idexam = reader["idexam"],
                iduser = reader["iduser"],
                mode = reader["mode"],
                title = reader["title"],
                nb_questions = reader["nb_questions"],
                duration = reader["duration"],
                exam_date = reader["exam_date"],
                score = reader["score"],
                answer_string = reader["answer_string"]
            });
        }

        return list;
    }

    public async Task<PilotExam> InsertAsync(InsertPilotExamRequest r)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var cmd = new NpgsqlCommand(@"
            INSERT INTO pilot_exam 
            (iduser, mode, title, nb_questions, duration, exam_date, score, answer_string)
            VALUES 
            (@iduser, @mode, @title, @nb_questions, @duration, @exam_date, @score, @answer_string)
            RETURNING *;
        ", conn);

        cmd.Parameters.AddWithValue("@iduser", r.IdUser);
        cmd.Parameters.AddWithValue("@mode", (object?)r.Mode ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@title", (object?)r.Title ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@nb_questions", r.NbQuestions);
        cmd.Parameters.AddWithValue("@duration", (object?)r.Duration ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@exam_date", (object?)r.ExamDate ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@score", r.Score);
        cmd.Parameters.AddWithValue("@answer_string", (object?)r.AnswerString ?? DBNull.Value);

        await using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new PilotExam
            {
                IdExam = reader.GetInt64(reader.GetOrdinal("idexam")),
                IdUser = reader.GetInt64(reader.GetOrdinal("iduser")),
                Mode = reader["mode"]?.ToString(),
                Title = reader["title"]?.ToString(),
                NbQuestions = reader.GetInt32(reader.GetOrdinal("nb_questions")),
                Duration = reader["duration"]?.ToString(),
                ExamDate = reader["exam_date"]?.ToString(),
                Score = reader["score"] == DBNull.Value ? 0 : Convert.ToInt32(reader["score"]),
                AnswerString = reader["answer_string"]?.ToString()
            };
        }

        throw new InvalidOperationException("Insert failed: no row returned.");
    }

    public async Task<object?> DeleteAsync(int idExam)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var cmd = new NpgsqlCommand(@"
            DELETE FROM pilot_exam
            WHERE idexam = @idexam
            RETURNING *
        ", conn);

        cmd.Parameters.AddWithValue("@idexam", idExam);

        await using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new
            {
                idexam = reader["idexam"],
                iduser = reader["iduser"],
                mode = reader["mode"],
                title = reader["title"],
                nb_questions = reader["nb_questions"],
                duration = reader["duration"],
                exam_date = reader["exam_date"],
                score = reader["score"],
                answer_string = reader["answer_string"]
            };
        }

        return null;
    }

    public async Task<int> ClearAllAsync(int idUser)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var cmd = new NpgsqlCommand(@"
            DELETE FROM pilot_exam
            WHERE iduser = @iduser
            RETURNING idexam
        ", conn);

        cmd.Parameters.AddWithValue("@iduser", idUser);

        await using var reader = await cmd.ExecuteReaderAsync();

        int count = 0;

        while (await reader.ReadAsync())
        {
            count++;
        }

        return count;
    }
}