using CourseWork.DTOs;
using CourseWork.Models;
using CourseWork.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Controllers;

[ApiController]
[Route("api/breeds")]
public class BreedController(IBreedService breedService) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetAllBreeds()
    {
        var breeds = await breedService.GetAllBreedsAsync();
    
        return Ok(breeds);
    }


    [HttpGet("{breedId}")]
    public async Task<IActionResult> GetBreedById(int breedId)
    {
        var breed = await breedService.GetBreedByIdAsync(breedId);
        
        return Ok(breed);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBreed([FromBody] BreedsDto breed)
    {
        var newBreed = await breedService.CreateBreedAsync(breed);
        
        return Ok(newBreed);
    }

    [HttpPut("{breedId}")]
    public async Task<IActionResult> UpdateBreed(int breedId, [FromBody] BreedsDto breed)
    {
        var isUpdated = await breedService.UpdateBreedAsync(breedId, breed);
        
        if (!isUpdated) 
        {
            return NotFound($"Породу з ID {breedId} не знайдено."); 
        }
    
        return NoContent();
    }

    [HttpGet("search")]
    public async Task<IActionResult> GetBreedsByName([FromQuery] string name)
    {
        var breeds = await breedService.GetBreedsByNameAsync(name);
        
        return Ok(breeds);
    }
    
    [HttpGet("unique-names")]
    public async Task<IActionResult> GetUniqueNames(
        [FromQuery] string? searchTerm = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var names = await breedService.GetUniqueBreedNamesAsync(searchTerm, pageNumber, pageSize);
        var totalCount = await breedService.GetUniqueBreedNamesCountAsync(searchTerm);

        return Ok(new
        {
            items = names, 
            totalCount,
            pageNumber,
            pageSize
        });
    }
}