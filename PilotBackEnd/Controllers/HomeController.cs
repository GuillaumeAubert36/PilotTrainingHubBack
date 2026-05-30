using Microsoft.AspNetCore.Mvc;

[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet("/")]
    public ContentResult Index()
    {
        var html = @"
        <h1>MY SERVER v2.1</h1>
        <table border='1' cellpadding='8'>
            <tr>
                <th>Name</th>
                <th>Endpoint</th>
            </tr>
            <tr>
                <td>LOGIN PILOT USER</td>
                <td>/api/auth/login</td>
            </tr>
            <tr>
                <td>REGISTER PILOT</td>
                <td>/api/auth/register</td>
            </tr>
            <tr>
                <td>SELECT PILOT EXAM</td>
                <td>/api/pilot-exam/select</td>
            </tr>
            <tr>
                <td>SELECT ONE PILOT EXAM</td>
                <td>/api/pilot-exam/select-one</td>
            </tr>
            <tr>
                <td>INSERT PILOT EXAM</td>
                <td>/api/pilot-exam/insert</td>
            </tr>
            <tr>
                <td>DELETE PILOT EXAM</td>
                <td>/api/pilot-exam/delete</td>
            </tr>
            <tr>
                <td>DELETE ALL PILOT EXAMS</td>
                <td>/api/pilot-exam/clear-all</td>
            </tr>
        </table>";

        return new ContentResult
        {
            Content = html,
            ContentType = "text/html"
        };
    }
}