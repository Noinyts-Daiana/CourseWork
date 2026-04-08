using CourseWork.DTOs;
using CourseWork.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Controllers;

[ApiController]
[Route("api/animals")] 
public class AnimalController(IAnimalService animalService) : ControllerBase 
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AnimalDto>>> GetAllAnimals(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 9,
        [FromQuery] string? searchTerm = null
        )
    {
        var animals = await animalService.GetAllAnimalsAsync(pageNumber, pageSize, searchTerm);
        return Ok(new
        {
            items = animals,
            totalCount = await animalService.GetAnimalsCountAsync(),
            pageNumber = pageNumber,
            pageSize = pageSize,
        }); 
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AnimalDto>> GetAnimalById(int id)
    {
        var animal = await animalService.GetAnimalByIdAsync(id);
        
        if (animal == null)
        {
            return NotFound(new { message = $"Тварину з ID {id} не знайдено." }); // Повертає 404
        }
        
        return Ok(animal);
    }
    [HttpPost]
    public async Task<ActionResult<AnimalDto>> AddAnimal([FromBody] AnimalDto animalDto)
    {
        var createdAnimal = await animalService.AddAnimalAsync(animalDto);
        
        return CreatedAtAction(nameof(GetAnimalById), new { id = createdAnimal.Id }, createdAnimal);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAnimal(int id, [FromBody] AnimalDto animalDto)
    {
        if (id != animalDto.Id)
        {
            return BadRequest(new { message = "ID в URL не співпадає з ID у тілі запиту." }); 
        }

        await animalService.UpdateAnimalAsync(animalDto);
        
        return NoContent(); 
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAnimal(int id)
    {
        await animalService.DeleteAnimalAsync(id);
        return NoContent(); 
    }
}