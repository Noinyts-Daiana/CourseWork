using CourseWork.DTOs;
using CourseWork.Services;
using CourseWork.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Controllers;

[ApiController]
[Route("api/adopt-animal")]
[Authorize]
public class AdoptAnimalController(IAdoptAnimalService adoptAnimalService) : ControllerBase
{
    [HttpGet("available")]
    [RequirePermission("ViewAnimals")]
    public async Task<ActionResult<IEnumerable<AdoptAnimalDto>>> GetAvailableAnimals()
    {
        var animals = await adoptAnimalService.GetAvailableAnimalsAsync();
        return Ok(animals);
    }

    [HttpGet("user/{ownerId}")]
    [RequirePermission("ViewAnimals")]
    public async Task<ActionResult<IEnumerable<AdoptAnimalDto>>> GetUserAdoptions(int ownerId)
    {
        var adoptions = await adoptAnimalService.GetUserAdoptionsAsync(ownerId);
        return Ok(adoptions);
    }

    [HttpPost("adopt")]
    [RequirePermission("AssignAnimal")]
    public async Task<IActionResult> Adopt([FromBody] AdoptAnimalDto request)
    {
        try
        {
            var result = await adoptAnimalService.AdoptAnimalAsync(request.AnimalId, request.OwnerId.Value);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("return")]
    [RequirePermission("ReturnAnimal")]
    public async Task<IActionResult> ReturnAnimal([FromBody] AdoptAnimalDto request)
    {
        try
        {
            var result = await adoptAnimalService.RegisterArrivalAsync(request.AnimalId, request.OwnerId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}