namespace CourseWork.Services;

public interface ITokenService
{
    void SetAuthCookie(string token);
    string GenerateJwtToken(string email, string role);
}