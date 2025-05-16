using Dapper;
using CourseSelectionSystem.Models;
using CourseSelectionSystem.Services;
using Microsoft.Data.SqlClient;

public class LoginService : ILoginService
{
    private readonly SqlConnection _con;

    public LoginService(SqlConnection con)
    {
        _con = con;
    }

    public async Task<StudentsModel?> ValidateStudentLoginAsync(string email, string password)
    {
        var sql = @"/**/
            SELECT * FROM students 
            WHERE email_ = @Email AND password_ = @Password AND delete_ = 0";

        return await _con.QueryFirstOrDefaultAsync<StudentsModel>(sql, new
        {
            Email = email,
            Password = HashHelper.ComputeSha256Base64(password)
        });
    }

    public async Task<TeachersModel?> ValidateTeacherLoginAsync(string email, string password)
    {
        var x = HashHelper.ComputeSha256Base64(password);
        var sql = @"/**/
            SELECT * FROM teachers 
            WHERE email_ = @Email AND password_ = @Password AND delete_ = 0";

        return await _con.QueryFirstOrDefaultAsync<TeachersModel>(sql, new
        {
            Email = email,
            Password = HashHelper.ComputeSha256Base64(password)
        });
    }
}
