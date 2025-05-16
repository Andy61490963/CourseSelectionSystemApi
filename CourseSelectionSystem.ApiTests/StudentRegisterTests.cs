using System.Net.Http.Json;
using CourseSelectionSystem.Models;
using FluentAssertions;
using Xunit;

namespace CourseSelectionSystem.ApiTests;

public class StudentRegisterTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public StudentRegisterTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    /// <summary>
    /// 測試註冊學生帳號
    /// </summary>
    [Fact(DisplayName = "POST /api/Student/Register - 成功建立新學生")]
    public async Task Register_ShouldReturn201_WhenNewStudent()
    {
        var payload = new StudentsModel
        {
            name_ = "測試學生",
            email_ = $"test_{Guid.NewGuid()}@test.com",
            password_ = "Test123456"
        };

        var response = await _client.PostAsJsonAsync("/api/Student/Register", payload);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var student = await response.Content.ReadFromJsonAsync<StudentsModel>();
        student.Should().NotBeNull();
        student.id_.Should().NotBe(Guid.Empty);
    }

    /// <summary>
    /// 測試重複註冊回傳的Status Code是否正確
    /// </summary>
    [Fact(DisplayName = "POST /api/Student/Register - Email 因為重複註冊的關係，已註冊應回傳 409")]
    public async Task Register_ShouldReturn409_WhenEmailExists()
    {
        var email = $"test_{Guid.NewGuid()}@test.com";

        var payload = new StudentsModel
        {
            name_ = "第一次註冊",
            email_ = email,
            password_ = "Test123456"
        };

        // 第一次註冊
        var response1 = await _client.PostAsJsonAsync("/api/Student/Register", payload);
        response1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // 第二次重複註冊
        var payload2 = new StudentsModel
        {
            name_ = "第二次註冊",
            email_ = email,
            password_ = "AnotherPassword"
        };

        var response2 = await _client.PostAsJsonAsync("/api/Student/Register", payload2);
        response2.StatusCode.Should().Be(System.Net.HttpStatusCode.Conflict);
    }
    
    /// <summary>
    /// 測學生列表
    /// </summary>
    [Fact(DisplayName = "GET /api/Student/GetAllStudents - 回傳學生列表")]
    public async Task GetAllStudents_ShouldReturnList()
    {
        var response = await _client.GetAsync("/api/Student/GetAllStudents");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var students = await response.Content.ReadFromJsonAsync<List<StudentsModel>>();
        students.Should().NotBeNull();
    }

    /// <summary>
    /// 測試取得單一學生
    /// </summary>
    [Fact(DisplayName = "GET /api/Student/GetOneById/{id} - 存在學生應回傳 200")]
    public async Task GetStudentById_ShouldReturn200_WhenFound()
    {
        var payload = new StudentsModel
        {
            name_ = "查詢用學生",
            email_ = $"find_{Guid.NewGuid()}@test.com",
            password_ = "Test123456"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Student/Register", payload);
        createResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var student = await createResponse.Content.ReadFromJsonAsync<StudentsModel>();

        var getResponse = await _client.GetAsync($"/api/Student/GetOneById/{student!.id_}");
        getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    /// <summary>
    /// 測試取得不存在學生
    /// </summary>
    [Fact(DisplayName = "GET /api/Student/GetOneById/{id} - 不存在應回傳 404")]
    public async Task GetStudentById_ShouldReturn404_WhenNotFound()
    {
        var response = await _client.GetAsync($"/api/Student/GetOneById/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}
