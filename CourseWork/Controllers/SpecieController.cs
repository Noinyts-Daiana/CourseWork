using CourseWork.DTOs;
using CourseWork.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpecieController(ISpecieService specieService): ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllSpecies()
    {
        var species = await specieService.GetAllSpeciesAsync();
        
        return Ok(species);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSpecies(int id)
    {
        var specie = await specieService.GetSpecieAsync(id);
        return Ok(specie);
    }

    [HttpPost]
    public async Task<IActionResult> AddSpecieAsync([FromBody] SpeciesDto species)
    {
        await specieService.AddSpecieAsync(species);

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSpecieAsync(int id, [FromBody] SpeciesDto species)
    {
        await specieService.UpdateSpecieAsync(id, species);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSpecieAsync(int id)
    {
        await specieService.DeleteSpecieAsync(id);
        return Ok();
    }
}