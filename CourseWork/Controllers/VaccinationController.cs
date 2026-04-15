using CourseWork.DTOs;
using CourseWork.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Controllers;


[ApiController]
[Route("api/vacctinations")]
public class VaccinationController(IVaccinationService vaccinationService): ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetVaccinationsAsync(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 9,
        [FromQuery] string? searchTerm = null)
    {
        var vaccinations = await vaccinationService.GetAllVaccinationsAsync(pageNumber, pageSize, searchTerm);
        return Ok( new{
            items = vaccinations,
            totalCount = await vaccinationService.GetVaccinationsCountAsync(),
            pageNumber = pageNumber,
            pageSize = pageSize});
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVaccinationAsync(int id, [FromBody] VaccinationDto vaccinationDto)
    {
        await vaccinationService.UpdateVaccinationAsync(id, vaccinationDto);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> AddVaccinationAsync([FromBody] VaccinationDto vaccinationDto)
    {
        await vaccinationService.AddVaccinationAsync(vaccinationDto);
        return Ok();
    }
}