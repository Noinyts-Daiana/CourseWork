using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using CourseWork.Models;
using CourseWork.DTOs;
using CourseWork.Repositories;
using CourseWork.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


namespace CourseWork.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(
   IUserService userService, 
   ITokenService tokenService, 
   ILogger<UsersController> logger) : ControllerBase
{
   [HttpGet]
   public async Task<IActionResult> GetUsers(
      [FromQuery] int pageNumber = 1, 
      [FromQuery] int pageSize = 9,
      [FromQuery] string? searchTerm = null,
      [FromQuery] int? roleId = null,
      [FromQuery] bool? isActive = null) 
   {
      var users = await userService.GetUsers(pageNumber, pageSize, searchTerm, roleId, isActive);
   
      var totalCount = await userService.GetTotalUsersCountAsync(searchTerm, roleId);
  
      return Ok(new {
         items = users,
         totalCount = totalCount,
         pageNumber = pageNumber,
         pageSize = pageSize
      });
   }

   [HttpGet("{id:int}")]
   public async Task<IActionResult> GetUserById(int id)
   {
      var user = await userService.GetUserById(id);
      return Ok(user);
   }

   [HttpGet("role/{roleId:int}")]
   public async Task<IActionResult> GetUserByRole(int roleId)
   {
      var user = await userService.GetUserByRole(roleId);
      return Ok(user);
   }
   
   [HttpPost("register")]
   public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
   {
      var createdUser = await userService.AddUser(userDto);

      var token = tokenService.GenerateJwtToken(createdUser.UserId, "User", 3); 
   
      tokenService.SetAuthCookie(token);
      
      return Ok(new 
      {
         User = createdUser,
         Message = "Реєстрація успішна!"
      });  
   }

   [HttpGet("me")]
   [Authorize]
   public async Task<IActionResult> GetMe()
   {
      var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
      logger.LogInformation("Спроба отримати профіль для ID із токена: '{Id}'", userIdString);

      var userId = int.Parse(userIdString);
      var user = await userService.GetUserById(userId);

      return Ok(user);
   }

   [HttpPut("{id:int}")]
   [Authorize]
   public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto userDto)
   {
      var user = await userService.UpdateUserAsync(id, userDto);
      return Ok(user);
   }

   [HttpPut("me")]
   [Authorize]
   public async Task<IActionResult> UpdateUser([FromBody] UserDto userDto)
   {
      var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
      
      var userId = int.Parse(userIdString);
      
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
   public async Task<IActionResult> AddUser([FromBody] UserDto userDto)
   {
      var user = await userService.AddUser(userDto);
      return Ok(user);
   }
   
   [HttpPatch("{id}/toggle-status")]
   public async Task<IActionResult> ToggleStatus(int id)
   {
      var success = await userService.ToggleUserStatusAsync(id);
    
      if (!success) return NotFound();
      
      var user = await userService.GetUserById(id); 

      return Ok(new { 
         isActive = user.IsActive, 
         message = "Статус користувача змінено" 
      });
   }
   /*var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

if (user == null || !VerifyPassword(dto.Password, user.Password)) 
    return Unauthorized("Невірний логін або пароль");

// ОСЬ ТУТ:
if (!user.IsActive) 
    return BadRequest(new { message = "Ваш акаунт деактивовано. Зверніться до адміністратора." });

var token = _tokenService.GenerateJwtToken(user.Id, user.Role.Name);*/
}


