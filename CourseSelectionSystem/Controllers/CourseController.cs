using CourseSelectionSystem.Enum;
using CourseSelectionSystem.Models;
using CourseSelectionSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = $"{nameof(UserTypeFlag.Student)},{nameof(UserTypeFlag.Teacher)}")]
public class CourseController : ControllerBase
{
    private readonly ICourseService _courseService;
    private readonly ITeacherService _teacherService;

    public CourseController(ICourseService courseService, ITeacherService teacherService)
    {
        _courseService = courseService;
        _teacherService = teacherService;
    }

    /// <summary>
    /// 所有課程列表
    /// </summary>
    [HttpGet("GetAllCourses")]
    [ProducesResponseType(typeof(IEnumerable<CourseModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCourses()
    {
        var result = await _courseService.GetAllCoursesAsync(limit: 100);
        return Ok(result);
    }

    /// <summary>
    /// 課程列表（含講師資訊）
    /// </summary>
    [HttpGet("GetCourseWithTeacher")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllWithTeachers()
    {
        var result = await _courseService.GetAllCoursesWithTeachersAsync(limit: 2);
        return Ok(result);
    }

    /// <summary>
    /// 指定講師開設課程
    /// </summary>
    [HttpGet("{teacherId}/GetCoursesByTeacherId")]
    [ProducesResponseType(typeof(IEnumerable<CourseModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCourses(Guid teacherId)
    {
        var result = await _teacherService.GetCoursesByTeacherAsync(teacherId, limit: 2);
        return Ok(result);
    }

    /// <summary>
    /// 建立新課程
    /// </summary>
    [HttpPost("CreateNewCourses")]
    [ProducesResponseType(typeof(CourseModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CourseModel course)
    {
        var newId = await _courseService.CreateCourseAsync(course);
        return CreatedAtAction(nameof(GetAllWithTeachers), new { id = newId }, course);
    }

    /// <summary>
    /// 更新課程內容
    /// </summary>
    [HttpPut("{courseId}/UpdateCourse")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid courseId, [FromBody] CourseModel course)
    {
        var success = await _courseService.UpdateCourseAsync(courseId, course);
        if (!success) return NotFound();
        return Ok();
    }

    /// <summary>
    /// 刪除課程
    /// </summary>
    [HttpDelete("{courseId}/DeleteCourse")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid courseId)
    {
        var success = await _courseService.DeleteCourseAsync(courseId);
        if (!success) return NotFound();
        return NoContent();
    }

    /// <summary>
    /// 根據課程 ID 取得學生清單
    /// </summary>
    [HttpGet("{courseId}/GetStudentsByCourseId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudents(Guid courseId)
    {
        var students = await _courseService.GetStudentsByCourseAsync(courseId);
        return Ok(students);
    }
}