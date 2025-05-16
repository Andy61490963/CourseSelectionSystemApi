using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace CourseSelectionSystem.ApiTests;

/// <summary>
/// 模擬起一個在 記憶體中執行的 Web API 主機（TestServer）
/// </summary>
public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
}