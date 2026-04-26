using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CourseWork.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;

public class TokenService : ITokenService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _config;

    public TokenService(IHttpContextAccessor httpContextAccessor, IConfiguration config)
    {
        _httpContextAccessor = httpContextAccessor;
        _config = config;
    }

    public string GenerateJwtToken(int userId, string roleName, int roleId)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Role, roleName),
            new Claim("roleId", roleId.ToString())  
        };
 
        var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
 
        var token = new JwtSecurityToken(
            issuer:             _config["Jwt:Issuer"],
            audience:           _config["Jwt:Audience"],
            claims:             claims,
            expires:            DateTime.Now.AddDays(7),
            signingCredentials: creds
        );
 
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public void SetAuthCookie(string token)
    {
        var response = _httpContextAccessor.HttpContext?.Response;
        if (response == null) return;

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false, 
            SameSite = SameSiteMode.Lax, 
            Expires = DateTime.UtcNow.AddDays(7),
            Path = "/" 
        };

        response.Cookies.Append("token", token, cookieOptions);
    }
}