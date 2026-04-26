using CourseWork.DTOs;
using CourseWork.Repositories.Interfaces;
using CourseWork.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Controllers;

[ApiController]
[Route("api/vacctinations")]
[Authorize]
public class VaccinationController(IVaccinationService vaccinationService) : ControllerBase
{
    [HttpGet]
    [RequirePermission("ViewVaccinations")]
    public async Task<IActionResult> GetVaccinationsAsync(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 9,
        [FromQuery] string? searchTerm = null)
    {
        var vaccinations = await vaccinationService.GetAllVaccinationsAsync(pageNumber, pageSize, searchTerm);
        return Ok(new
        {
            items = vaccinations,
            totalCount = await vaccinationService.GetVaccinationsCountAsync(searchTerm),
            pageNumber = pageNumber,
            pageSize = pageSize
        });
    }

    [HttpPost]
    [RequirePermission("AddVaccination")]
    public async Task<IActionResult> AddVaccinationAsync([FromBody] VaccinationDto vaccinationDto)
    {
        await vaccinationService.AddVaccinationAsync(vaccinationDto);
        return Ok();
    }

    [HttpPut("{id}")]
    [RequirePermission("EditVaccination")]
    public async Task<IActionResult> UpdateVaccinationAsync(int id, [FromBody] VaccinationDto vaccinationDto)
    {
        await vaccinationService.UpdateVaccinationAsync(id, vaccinationDto);
        return Ok();
    }

    [HttpDelete("{id}")]
    [RequirePermission("DeleteVaccination")]
    public async Task<IActionResult> DeleteVaccinationAsync(int id)
    {
        await vaccinationService.DeleteVaccinationAsync(id);
        return Ok();
    }
}