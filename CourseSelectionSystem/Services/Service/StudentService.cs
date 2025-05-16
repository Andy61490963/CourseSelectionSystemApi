using Dapper;
using CourseSelectionSystem.Models;
using CourseSelectionSystem.Services;
using Microsoft.Data.SqlClient;

public class StudentService : IStudentService
{
    private readonly SqlConnection _con;

    public StudentService(SqlConnection con)
    {
        _con = con;
    }

    /// <summary>
    /// 建立學生（註冊）
    /// </summary>
    public async Task<Guid?> CreateStudentAsync(StudentsModel student)
    {
        var existsSql = @"/**/
SELECT TOP 1 1 FROM students WHERE email_ = @Email AND delete_ = 0
";
        var exists = await _con.ExecuteScalarAsync<int>(existsSql, new { Email = student.email_ });
        if (exists > 0)
        {
            return null;
        }

        var id_ = Guid.NewGuid();
        student.password_ = HashHelper.ComputeSha256Base64(student.password_);

        var sql = @"/**/
INSERT INTO students (id_, name_, email_, password_, create_time_, delete_)
VALUES (@id_, @name_, @email_, @password_, @create_time_, 0)
";
        await _con.ExecuteAsync(sql, new
        {
            id_,
            student.name_,
            student.email_,
            student.password_,
            create_time_ = DateTime.UtcNow
        });

        return id_;
    }
    
    /// <summary>
    /// 取得所有學生（最多 limit 筆）
    /// </summary>
    public async Task<IEnumerable<StudentsModel>> GetAllStudentsAsync(int limit)
    {
        var sql = @"/**/
SELECT TOP (@Limit) *
FROM students
WHERE delete_ = 0
ORDER BY create_time_ DESC";

        return await _con.QueryAsync<StudentsModel>(sql, new { Limit = limit });
    }

    /// <summary>
    /// 取得 單一
    /// </summary>
    /// <param name="studentId"></param>
    /// <returns></returns>
    public async Task<StudentsModel?> GetStudentByIdAsync(Guid studentId)
    {
        var sql = @"/**/
SELECT * FROM students WHERE id_ = @Id AND delete_ = 0
";
        return await _con.QueryFirstOrDefaultAsync<StudentsModel>(sql, new { Id = studentId });
    }

    /// <summary>
    /// 查詢學生已選課程（最多 limit 筆）
    /// </summary>
    public async Task<IEnumerable<CourseModel>> GetSelectedCoursesAsync(Guid studentId, int limit)
    {
        var sql = @"/**/
SELECT TOP (@Limit) c.*
FROM courses c
JOIN course_selection cs ON c.id_ = cs.course_id_
WHERE cs.student_id_ = @studentId
ORDER BY cs.select_time_ DESC";

        return await _con.QueryAsync<CourseModel>(sql, new { studentId, limit });
    }

    /// <summary>
    /// 更新學生選課（先刪後新增）
    /// </summary>
    public async Task<bool> UpdateCourseSelectionsAsync(Guid studentId, List<Guid> courseIds)
    {
        using var tx = _con.BeginTransaction();

        try
        {
            // 刪除舊的選課資料
            var delSql = @"/**/
UPDATE course_selection SET delete_ = 1 WHERE student_id_ = @studentId
";
            await _con.ExecuteAsync(delSql, new { studentId }, tx);

            // 新增新的選課資料
            var insSql = @"/**/
INSERT INTO course_selection (id_, student_id_, course_id_, select_time_, delete_)
VALUES (@id_, @student_id_, @course_id_, @select_time_, @delete_)
";

            foreach (var courseId in courseIds)
            {
                await _con.ExecuteAsync(insSql, new
                {
                    id_ = Guid.NewGuid(),
                    student_id_ = studentId,
                    course_id_ = courseId,
                    select_time_ = DateTime.UtcNow,
                    delete_ = false
                }, tx);
            }

            tx.Commit();
            return true;
        }
        catch
        {
            tx.Rollback();
            return false;
        }
    }
}