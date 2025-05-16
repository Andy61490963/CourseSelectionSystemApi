using CourseSelectionSystem.Enum;
using CourseSelectionSystem.Models;
using CourseSelectionSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TeacherController : ControllerBase
{
    private readonly ITeacherService _teacherService;

    public TeacherController(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    /// <summary>
    /// 建立講師（註冊）
    /// </summary>
    [AllowAnonymous]
    [HttpPost("Register")]
    [ProducesResponseType(typeof(TeachersModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] TeachersModel teacher)
    {
        var newId = await _teacherService.CreateTeacherAsync(teacher);
        if (newId == null)
        {
            return Conflict("Email 已被註冊");
        }

        teacher.id_ = newId.Value;
        return CreatedAtAction(nameof(GetAll), new { id = newId }, teacher);
    }

    /// <summary>
    /// 取得所有講師（最多兩筆）
    /// </summary>
    [HttpGet("GetAllTeachers")]
    [ProducesResponseType(typeof(IEnumerable<TeachersModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _teacherService.GetAllTeachersAsync(limit: 2);
        return Ok(result);
    }

    /// <summary>
    /// 根據講師 ID 取得單一講師
    /// </summary>
    [HttpGet("GetOneById/{teacherId}")]
    [ProducesResponseType(typeof(TeachersModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid teacherId)
    {
        var teacher = await _teacherService.GetTeacherByIdAsync(teacherId);
        if (teacher == null)
            return NotFound();
        return Ok(teacher);
    }

    /// <summary>
    /// 更新講師資訊
    /// </summary>
    [HttpPut("{teacherId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid teacherId, [FromBody] TeachersModel updatedTeacher)
    {
        var success = await _teacherService.UpdateTeacherAsync(teacherId, updatedTeacher);
        if (!success)
            return NotFound();
        return NoContent();
    }

    /// <summary>
    /// 刪除講師
    /// </summary>
    [HttpDelete("{teacherId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid teacherId)
    {
        var success = await _teacherService.DeleteTeacherAsync(teacherId);
        if (!success)
            return NotFound();
        return NoContent();
    }

    /// <summary>
    /// 取得講師所開課程（最多兩筆）
    /// </summary>
    [HttpGet("GetTeachersCourse/{teacherId}")]
    [ProducesResponseType(typeof(IEnumerable<CourseModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourses(Guid teacherId)
    {
        var result = await _teacherService.GetCoursesByTeacherAsync(teacherId, limit: 2);
        return Ok(result);
    }
}