namespace CourseWork.Services;

public interface ITokenService
{
    void SetAuthCookie(string token);
    string GenerateJwtToken(int id, string role);
}