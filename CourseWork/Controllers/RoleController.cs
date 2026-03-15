using Microsoft.AspNetCore.Mvc;
using CourseWork.Models; // Переконайся, що цей using є

namespace CourseWork.Controllers;

[ApiController]
[Route("api/roles")]
public class RolesController : ControllerBase // Додано public
{
    private readonly AppDbContext _context;
    
    public RolesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetRoles()
    {
        var roles = _context.Roles.ToList();
        return Ok(roles);
    }

    [HttpGet("{id}")]
    public IActionResult GetRole(int id)
    {
        var role = _context.Roles.Find(id);
        return Ok(role);
    }
}