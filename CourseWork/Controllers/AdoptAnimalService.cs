using CourseWork.DTOs;
using CourseWork.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Controllers;

[ApiController]
[Route("api/[controller]")] 
public class AdoptAnimalController(IAdoptAnimalService adoptAnimalService) : ControllerBase
{
    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<AdoptAnimalDto>>> GetAvailableAnimals()
    {
        var animals = await adoptAnimalService.GetAvailableAnimalsAsync();
        return Ok(animals); 
    }

    [HttpGet("user/{ownerId}")]
    public async Task<ActionResult<IEnumerable<AdoptAnimalDto>>> GetUserAdoptions(int ownerId)
    {
        var adoptions = await adoptAnimalService.GetUserAdoptionsAsync(ownerId);
        return Ok(adoptions);
    }

    [HttpPost("{animalId}/arrival")]
    public async Task<ActionResult<AdoptAnimalDto>> RegisterArrival(int animalId, [FromQuery] DateTime? date = null)
    {
        try
        {
            var result = await adoptAnimalService.RegisterArrivalAsync(animalId, date);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message }); 
        }
    }

    [HttpPost("{animalId}/adopt")]
    public async Task<ActionResult<AdoptAnimalDto>> AdoptAnimal(int animalId, [FromQuery] int ownerId, [FromQuery] DateTime? date = null)
    {
        try
        {
            var result = await adoptAnimalService.AdoptAnimalAsync(animalId, ownerId, date);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message }); 
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message }); 
        }
    }
}