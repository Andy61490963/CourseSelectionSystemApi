using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CourseSelectionSystem.Helper;

public static class JwtTokenHelper
{
    /// <summary>
    /// 產生 JWT Token
    /// </summary>
    /// <param name="email">使用者 Email（用來識別身份）</param>
    /// <param name="role">使用者角色（例如 Teacher 或 Student）</param>
    /// <param name="config">注入的 IConfiguration，用來讀取 appsettings.json 的 Jwt 設定</param>
    /// <returns>JWT Token 字串</returns>
    public static string GenerateToken(string email, string role, IConfiguration config)
    {
        // 建立 JWT 的 Claims
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role)
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 建立 JWT Token 實例
        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],                         // 發行者
            audience: config["Jwt:Audience"],                     // 使用者
            claims: claims,                                       // 身分資料
            expires: DateTime.UtcNow.AddMinutes(
                int.Parse(config["Jwt:ExpireMinutes"])            // 過期時間
            ),
            signingCredentials: creds                             // 金鑰簽章
        );

        // 將 JWT Token 實體序列化成字串
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}