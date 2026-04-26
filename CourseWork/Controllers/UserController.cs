using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using CourseWork.DTOs;
using CourseWork.Services;
using CourseWork.Attributes;
using Microsoft.AspNetCore.Authorization;
using CourseWork.Repositories.Interfaces;

namespace CourseWork.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(
    IUserService userService,
    ITokenService tokenService,
    IPermissionService permissionService,
    IUserRepository userRepository,
    ILogger<UsersController> logger) : ControllerBase
{
    [HttpGet]
    [Authorize]
    [RequirePermission("EditUser")]
    public async Task<IActionResult> GetUsers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 9,
        [FromQuery] string? searchTerm = null,
        [FromQuery] int? roleId = null,
        [FromQuery] bool? isActive = null)
    {
        var users = await userService.GetUsers(pageNumber, pageSize, searchTerm, roleId, isActive);
        var totalCount = await userService.GetTotalUsersCountAsync(searchTerm, roleId);

        return Ok(new
        {
            items = users,
            totalCount = totalCount,
            pageNumber = pageNumber,
            pageSize = pageSize
        });
    }

    [HttpGet("{id:int}")]
    [Authorize]
    [RequirePermission("EditUser")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await userService.GetUserById(id);
        return Ok(user);
    }

    [HttpGet("role/{roleId:int}")]
    [Authorize]
    [RequirePermission("EditUser")]
    public async Task<IActionResult> GetUserByRole(int roleId)
    {
        var user = await userService.GetUserByRole(roleId);
        return Ok(user);
    }

    [HttpPost("register")]
    public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
    {
        var createdUser = await userService.AddUser(userDto);
        var permissions = await permissionService.GetRolePermissionsAsync(createdUser.RoleId);
        var token = tokenService.GenerateJwtToken(createdUser.UserId, createdUser.RoleName ?? "User", createdUser.RoleId);
        tokenService.SetAuthCookie(token);

        return Ok(new
        {
            createdUser.UserId,
            createdUser.FullName,
            createdUser.Email,
            createdUser.RoleId,
            createdUser.RoleName,
            createdUser.IsActive,
            permissions,
            Message = "Реєстрація успішна!"
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await userRepository.GetByEmailAsync(dto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            return Unauthorized(new { message = "Невірний email або пароль" });

        if (!user.IsActive)
            return Unauthorized(new { message = "Обліковий запис деактивовано" });

        var permissions = await permissionService.GetRolePermissionsAsync(user.RoleId);
        var token = tokenService.GenerateJwtToken(user.Id, user.Role?.Name ?? "User", user.RoleId);
        tokenService.SetAuthCookie(token);

        return Ok(new
        {
            userId = user.Id,
            fullName = user.FullName,
            email = user.Email,
            roleId = user.RoleId,
            roleName = user.Role?.Name,
            isActive = user.IsActive,
            permissions
        });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("token", new CookieOptions { Path = "/" });
        return Ok(new { message = "Вихід успішний" });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMe()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        logger.LogInformation("Спроба отримати профіль для ID із токена: '{Id}'", userIdString);

        var userId = int.Parse(userIdString!);
        var user = await userService.GetUserById(userId);
        if (user == null) return NotFound();

        var permissions = await permissionService.GetRolePermissionsAsync(user.RoleId);

        return Ok(new
        {
            userId = user.UserId,
            fullName = user.FullName,
            email = user.Email,
            roleId = user.RoleId,
            roleName = user.RoleName,
            isActive = user.IsActive,
            permissions
        });
    }

    [HttpPut("me")]
    [Authorize]
    public async Task<IActionResult> UpdateMe([FromBody] UserDto userDto)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = int.Parse(userIdString!);
        await userService.UpdateUserAsync(userId, userDto);
        return Ok();
    }

    [HttpPut("me/password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = int.Parse(userIdString!);
        await userService.UpdatePassword(userId, dto);
        return Ok(new { message = "Пароль успішно змінено!" });
    }

    [HttpPost]
    [Authorize]
    [RequirePermission("AddUser")]
    public async Task<IActionResult> AddUser([FromBody] UserDto userDto)
    {
        var user = await userService.AddUser(userDto);
        
        return Ok(user);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    [RequirePermission("EditUser")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto userDto)
    {
        var user = await userService.UpdateUserAsync(id, userDto);
        return Ok(user);
    }

    [HttpPatch("{id}/toggle-status")]
    [Authorize]
    [RequirePermission("DeactivateUser")]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        var success = await userService.ToggleUserStatusAsync(id);

        if (!success) return NotFound();

        var user = await userService.GetUserById(id);
        return Ok(new
        {
            isActive = user.IsActive,
            message = "Статус користувача змінено"
        });
    }
}