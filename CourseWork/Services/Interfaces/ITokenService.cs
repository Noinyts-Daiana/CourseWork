namespace CourseWork.Services;

public interface ITokenService
{
    void SetAuthCookie(string token);
    string GenerateJwtToken(int userId, string roleName, int roleId);
}