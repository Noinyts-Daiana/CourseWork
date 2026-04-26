using CourseWork.DTOs;
using CourseWork.Services;
using CourseWork.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Controllers;

[ApiController]
[Route("api/animals")]
[Authorize]
public class AnimalController(IAnimalService animalService) : ControllerBase
{
    [HttpGet]
    [RequirePermission("ViewAnimals")]
    public async Task<ActionResult> GetAllAnimals(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 8,
        [FromQuery] string? searchTerm = null,
        [FromQuery] List<int>? charIds = null,
        [FromQuery] int? speciesId = null,
        [FromQuery] int? breedId = null,
        [FromQuery] int? sex = null,
        [FromQuery] bool? isAdopted = null)
    {
        var animals = await animalService.GetAllAnimalsAsync(
            pageNumber, pageSize, searchTerm, charIds, speciesId, breedId, sex, isAdopted);

        var totalFilteredCount = await animalService.GetAnimalsCountAsync(
            searchTerm, charIds, speciesId, breedId, sex, isAdopted);

        return Ok(new
        {
            items = animals,
            totalCount = totalFilteredCount,
            pageNumber = pageNumber,
            pageSize = pageSize,
        });
    }

    [HttpGet("{id}")]
    [RequirePermission("ViewAnimals")]
    public async Task<ActionResult<AnimalDto>> GetAnimalById(int id)
    {
        var animal = await animalService.GetAnimalByIdAsync(id);

        if (animal == null)
            return NotFound(new { message = $"Тварину з ID {id} не знайдено." });

        return Ok(animal);
    }

    [HttpPost]
    [RequirePermission("AddAnimal")]
    public async Task<ActionResult<AnimalDto>> AddAnimal([FromBody] AnimalDto animalDto)
    {
        var createdAnimal = await animalService.AddAnimalAsync(animalDto);
        return CreatedAtAction(nameof(GetAnimalById), new { id = createdAnimal.Id }, createdAnimal);
    }

    [HttpPut("{id}")]
    [RequirePermission("EditAnimal")]
    public async Task<IActionResult> UpdateAnimal(int id, [FromBody] AnimalDto animalDto)
    {
        if (id != animalDto.Id)
            return BadRequest(new { message = "ID в URL не співпадає з ID у тілі запиту." });

        await animalService.UpdateAnimalAsync(animalDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [RequirePermission("DeleteAnimal")]
    public async Task<IActionResult> DeleteAnimal(int id)
    {
        await animalService.DeleteAnimalAsync(id);
        return NoContent();
    }
}