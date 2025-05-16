using Dapper;
using CourseSelectionSystem.Models;
using CourseSelectionSystem.Services;
using Microsoft.Data.SqlClient;

public class CourseService : ICourseService
{
    private readonly SqlConnection _con;

    public CourseService(SqlConnection con)
    {
        _con = con;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="limit"></param>
    /// <returns></returns>
    public async Task<IEnumerable<CourseModel>> GetAllCoursesAsync(int limit)
    {
        var sql = @"/**/SELECT TOP (@Limit) * FROM courses WHERE delete_ = 0";
        return await _con.QueryAsync<CourseModel>(sql, new { Limit = limit });
    }
    
    /// <summary>
    /// 取得所有課程
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<CourseWithTeacherViewModel>> GetAllCoursesWithTeachersAsync(int limit)
    {
        var sql = @"/**/
SELECT TOP (@limit)
    c.id_ AS id_, c.name_ AS name_, c.description_, c.start_time_, c.end_time_, 
    c.teacher_id_, c.create_time_,
    t.id_ AS id_, t.name_ AS name_, t.email_ AS email_
FROM courses c
JOIN teachers t ON c.teacher_id_ = t.id_
WHERE c.delete_ = 0 AND t.delete_ = 0
ORDER BY c.create_time_ DESC";

        return await _con.QueryAsync<CourseWithTeacherViewModel, TeacherBriefViewModel, CourseWithTeacherViewModel>(
            sql,
            (course, teacher) =>
            {
                course.teacher = teacher;
                return course;
            },
            new { limit },
            splitOn: "id_"
        );
    }

    /// <summary>
    /// 建立新課程
    /// </summary>
    /// <param name="course"></param>
    /// <returns></returns>
    public async Task<Guid> CreateCourseAsync(CourseModel course)
    {
        var id = Guid.NewGuid();
        var sql = @"/**/
INSERT INTO courses (id_, name_, description_, start_time_, end_time_, teacher_id_, create_time_)
VALUES (@id_, @name_, @description_, @start_time_, @end_time_, @teacher_id_, @create_time_)
";
        await _con.ExecuteAsync(sql, new
        {
            id_ = id,
            course.name_,
            course.description_,
            course.start_time_,
            course.end_time_,
            course.teacher_id_,
            create_time_ = DateTime.Now
        });
        return id;
    }

    /// <summary>
    /// 更新課程內容
    /// </summary>
    /// <param name="id"></param>
    /// <param name="course"></param>
    /// <returns></returns>
    public async Task<bool> UpdateCourseAsync(Guid id, CourseModel course)
    {
        var sql = @"/**/
UPDATE courses
SET name_ = @name_, description_ = @description_, start_time_ = @start_time_, end_time_ = @end_time_, teacher_id_ = @teacher_id_
WHERE id_ = @id_";
        
        var affected = await _con.ExecuteAsync(sql, new
        {
            id_ = id,
            course.name_,
            course.description_,
            course.start_time_,
            course.end_time_,
            course.teacher_id_
        });
        return affected > 0;
    }

    /// <summary>
    /// 刪除課程(非物理性)
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> DeleteCourseAsync(Guid id)
    {
        var sql = @"/**/
UPDATE courses SET delete_ = 1 WHERE id_ = @id
";
        
        var affected = await _con.ExecuteAsync(sql, new { id });
        return affected > 0;
    }

    /// <summary>
    /// 取得參與這堂課程的學生
    /// </summary>
    /// <param name="courseId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<StudentsModel>> GetStudentsByCourseAsync(Guid courseId)
    {
        var sql = @"/**/
SELECT s.* FROM students s
JOIN course_selection cs ON s.id = cs.student_id_
WHERE cs.course_id_ = @courseId";

        return await _con.QueryAsync<StudentsModel>(sql, new { courseId });
    }

}
