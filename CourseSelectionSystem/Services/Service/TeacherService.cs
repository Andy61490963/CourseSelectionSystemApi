using Dapper;
using CourseSelectionSystem.Models;
using CourseSelectionSystem.Services;
using Microsoft.Data.SqlClient;

public class TeacherService : ITeacherService
{
    private readonly SqlConnection _con;

    public TeacherService(SqlConnection con)
    {
        _con = con;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="limit"></param>
    /// <returns></returns>
    public async Task<IEnumerable<TeachersModel>> GetAllTeachersAsync(int limit)
    {
        var sql = @"/**/
SELECT TOP (@Limit) * FROM teachers WHERE delete_ = 0 ORDER BY create_time_ DESC
";
        
        return await _con.QueryAsync<TeachersModel>(sql, new { Limit = limit });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="teacher"></param>
    /// <returns></returns>
    public async Task<Guid?> CreateTeacherAsync(TeachersModel teacher)
    {
        var existsSql = @"
SELECT TOP 1 1 FROM teachers 
WHERE email_ = @Email AND delete_ = 0";

        var exists = await _con.ExecuteScalarAsync<int?>(existsSql, new { Email = teacher.email_ });

        if (exists != null)
        {
            return null; // 已註冊
        }

        var id_ = Guid.NewGuid();
        teacher.password_ = HashHelper.ComputeSha256Base64(teacher.password_);

        var sql = @"
INSERT INTO teachers (id_, name_, email_, password_, create_time_, last_course_time_, delete_)
VALUES (@Id, @Name, @Email, @Password, @CreateTime, @LastCourseTime, 0)";

        await _con.ExecuteAsync(sql, new
        {
            Id = id_,
            Name = teacher.name_,
            Email = teacher.email_,
            Password = teacher.password_,
            CreateTime = DateTime.UtcNow,
            LastCourseTime = DateTime.UtcNow
        });

        return id_;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="teacherId"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    public async Task<IEnumerable<CourseModel>> GetCoursesByTeacherAsync(Guid teacherId, int limit)
    {
        var sql = @"/**/
SELECT TOP (@Limit) * FROM courses
WHERE teacher_id_ = @teacherId
ORDER BY create_time_ DESC
";

        return await _con.QueryAsync<CourseModel>(sql, new { teacherId, Limit = limit });
    }
    
    /// <summary>
    /// 取得 單一
    /// </summary>
    /// <param name="teacherId"></param>
    /// <returns></returns>
    public async Task<TeachersModel?> GetTeacherByIdAsync(Guid teacherId)
    {
        var sql = @"/**/
SELECT * FROM teachers WHERE id_ = @Id AND delete_ = 0
";
        return await _con.QueryFirstOrDefaultAsync<TeachersModel>(sql, new { Id = teacherId });
    }

    public async Task<bool> UpdateTeacherAsync(Guid teacherId, TeachersModel updatedTeacher)
    {
        var existing = await GetTeacherByIdAsync(teacherId);
        if (existing == null)
        {
            return false;
        }
        
        if (existing.name_ == updatedTeacher.name_ && existing.email_ == updatedTeacher.email_)
        {
            return true; 
        }

        var sql = @"/**/
        UPDATE teachers
        SET name_ = @Name,
            email_ = @Email,
            modify_time_ = @ModifyTime
        WHERE id_ = @Id AND delete_ = 0";

        var result = await _con.ExecuteAsync(sql, new
        {
            Id = teacherId,
            Name = updatedTeacher.name_,
            Email = updatedTeacher.email_,
            ModifyTime = DateTime.UtcNow
        });

        return result > 0;
    }
    
    /// <summary>
    /// 刪除
    /// </summary>
    /// <param name="teacherId"></param>
    /// <returns></returns>
    public async Task<bool> DeleteTeacherAsync(Guid teacherId)
    {
        var sql = @"/**/
UPDATE teachers SET delete_ = 1 WHERE id_ = @Id
";
        var rows = await _con.ExecuteAsync(sql, new { Id = teacherId });
        return rows > 0;
    }
}