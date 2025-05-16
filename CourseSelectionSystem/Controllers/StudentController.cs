using CourseSelectionSystem.Enum;
using CourseSelectionSystem.Models;
using CourseSelectionSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    /// <summary>
    /// 建立學生（註冊）
    /// </summary>
    [AllowAnonymous]
    [HttpPost("Register")]
    [ProducesResponseType(typeof(StudentsModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] StudentsModel student)
    {
        var newId = await _studentService.CreateStudentAsync(student);
        if (newId == null)
            return Conflict("Email 已被註冊");

        student.id_ = newId.Value;
        return CreatedAtAction(nameof(GetAll), new { id = newId }, student);
    }

    /// <summary>
    /// 取得所有學生（最多兩筆）
    /// </summary>
    [AllowAnonymous]
    [HttpGet("GetAllStudents")]
    [ProducesResponseType(typeof(IEnumerable<StudentsModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _studentService.GetAllStudentsAsync(limit: 2);
        return Ok(result);
    }

    /// <summary>
    /// 取得單一學生
    /// </summary>
    [HttpGet("GetOneById/{studentId}")]
    [ProducesResponseType(typeof(StudentsModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid studentId)
    {
        var student = await _studentService.GetStudentByIdAsync(studentId);
        if (student == null)
            return NotFound();
        return Ok(student);
    }

    /// <summary>
    /// 查詢學生已選課程（最多兩筆）
    /// </summary>
    [HttpGet("GetStudentsCourses/{studentId}")]
    [ProducesResponseType(typeof(IEnumerable<CourseModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSelectedCourses(Guid studentId)
    {
        var result = await _studentService.GetSelectedCoursesAsync(studentId, limit: 2);
        return Ok(result);
    }

    /// <summary>
    /// 修改學生選課（綁定講師與課程）
    /// </summary>
    [HttpPut("ModifyStudentCourseSelection/{studentId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateSelectedCourses(Guid studentId, [FromBody] List<Guid> courseIds)
    {
        var success = await _studentService.UpdateCourseSelectionsAsync(studentId, courseIds);
        if (!success) return NotFound();
        return Ok();
    }
}