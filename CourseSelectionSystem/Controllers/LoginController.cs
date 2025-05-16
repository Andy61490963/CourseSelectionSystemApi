using CourseSelectionSystem.Enum;
using CourseSelectionSystem.Helper;
using CourseSelectionSystem.Models;
using CourseSelectionSystem.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly ICourseService _courseService;
    private readonly ITeacherService _teacherService;
    private readonly ILoginService _loginService;
    private readonly IConfiguration _iConfiguration;

    public LoginController(ICourseService courseService, ITeacherService teacherService, ILoginService loginService, IConfiguration iConfiguration)
    {
        _courseService = courseService;
        _teacherService = teacherService;
        _loginService = loginService;
        _iConfiguration = iConfiguration;
    }

    /// <summary>
    /// 教師登入
    /// </summary>
    [HttpPost("TeacherLogin")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> TeacherLogin([FromBody] LoginModel req)
    {
        if (string.IsNullOrWhiteSpace(req.email) || string.IsNullOrWhiteSpace(req.password))
            return BadRequest("Email 與密碼不得為空");

        var result = await _loginService.ValidateTeacherLoginAsync(req.email, req.password);
        if (result == null)
        {
            return Unauthorized("帳號或密碼錯誤");
        }

        var token = JwtTokenHelper.GenerateToken(req.email, nameof(UserTypeFlag.Teacher), _iConfiguration);
        return Ok(new LoginResponse
        {
            name_ = result.name_,
            email_ = result.email_,
            role_ = nameof(UserTypeFlag.Teacher),
            token = token
        });
    }

    /// <summary>
    /// 學生登入
    /// </summary>
    [HttpPost("StudentLogin")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> StudentLogin([FromBody] LoginModel req)
    {
        if (string.IsNullOrWhiteSpace(req.email) || string.IsNullOrWhiteSpace(req.password))
            return BadRequest("Email 與密碼不得為空");

        var result = await _loginService.ValidateStudentLoginAsync(req.email, req.password);
        if (result == null)
        {
            return Unauthorized("帳號或密碼錯誤");
        }

        var token = JwtTokenHelper.GenerateToken(req.email, nameof(UserTypeFlag.Student), _iConfiguration);
        return Ok(new LoginResponse
        {
            name_ = result.name_,
            email_ = result.email_,
            role_ = nameof(UserTypeFlag.Student),
            token = token
        });
    }
}