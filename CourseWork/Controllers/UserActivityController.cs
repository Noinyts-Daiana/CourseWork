using CourseWork.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Controllers;

[ApiController]
[Route("api/user-activity")]
public class UserActivityController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetActivity()
    {
        try
        {
            var arrivals = await context.AdoptAnimal
                .Include(a => a.Animal)
                .Select(a => new {
                    Date = (DateTime?)a.ArrivalDate,
                    Description = $"Тварина {a.Animal.Name} прибула до притулку",
                    Type = "AnimalArrival",
                    User = "Система"
                }).ToListAsync();

            var adoptions = await (from a in context.AdoptAnimal
                join u in context.User on a.OwnerId equals u.Id into userGroup
                from u in userGroup.DefaultIfEmpty()
                select new {
                    Date = a.AdoptDate,
                    Description = $"{a.Animal.Name} знайшла нову родину",
                    Type = "AnimalAdoption",
                           
                    User = u != null ? u.FullName : "Невідомий власник"
                }).ToListAsync();
            
            var userRegistrations = await context.User 
                .Select(u => new {
                    Date = (DateTime?)u.CreatedAt,
                    Description = $"Новий користувач зареєстрований: {u.FullName ?? u.Email}",
                    Type = "UserRegistration",
                    User = "Система"
                }).ToListAsync();

            var userUpdates = await context.User
                .Where(u => u.UpdatedAt != null)
                .Select(u => new {
                    Date = u.UpdatedAt,
                    Description = $"Користувач {u.FullName ?? u.Email} оновив свій профіль",
                    Type = "UserProfileUpdate",
                    User = u.FullName ?? "Користувач"
                }).ToListAsync();

            var result = arrivals
                .Concat(adoptions)
                .Concat(userRegistrations)
                .Concat(userUpdates)
                .Where(x => x.Date != null)
                .OrderByDescending(x => x.Date)
                .Take(50)
                .ToList();

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}