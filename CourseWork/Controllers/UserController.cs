using Microsoft.AspNetCore.Mvc;
using CourseWork.Models;
using CourseWork.DTOs;
using CourseWork.Services;
using Microsoft.EntityFrameworkCore;


namespace CourseWork.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(IUserService userService, ITokenService tokenService): ControllerBase
{
   [HttpGet]
   public async Task<IActionResult> GetUsers()
   {
      var users = await userService.GetUsers();
      return Ok(users);
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
      try 
      {
         var createdUser = await userService.AddUser(userDto);

         var token = tokenService.GenerateJwtToken(userDto.Email, "User");
         tokenService.SetAuthCookie(token);
      
         return Ok(new 
         {
            User = createdUser,
            Message = "Реєстрація успішна!"
         });
      }
      catch (InvalidOperationException ex) 
      {
         return BadRequest(new { message = ex.Message });
      }
      catch (DbUpdateException ex) when (ex.InnerException is Npgsql.PostgresException { SqlState: "23505" })
      {
         return Conflict(new { message = "Користувач із такою електронною поштою вже зареєстрований." });
      }
      catch (Exception ex)
      {
         return StatusCode(500, new { message = "Сталася помилка на сервері: " + ex.Message });
      }
   }
}


