using Microsoft.AspNetCore.Mvc;
using CourseWork.Models;
using CourseWork.DTOs;


namespace CourseWork.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] UserCreateDto model)
    {
        var newUser = new User 
        {
            FullName = model.FullName,
            Email = model.Email,
            Password = model.Password, 
            RoleId = model.RoleId
        };

        _context.Users.Add(newUser);
        _context.SaveChanges();

        return Ok(new { message = "Створено!" });
    }
}


