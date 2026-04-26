using System.Security.Claims;
using CourseWork.DTOs;
using CourseWork.Services;
using CourseWork.Attributes;
using CourseWork.Repositories;
using CourseWork.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Controllers;

[ApiController]
[Route("api/adopt-animal")]
[Authorize]
public class AdoptAnimalController(
    IAdoptAnimalService adoptAnimalService,
    ISystemAlertService alertService,
    IUserRepository userRepository,
    IAnimalRepository animalRepository) : ControllerBase
{
    [HttpPost("request")]
    [RequirePermission("CreateAdoptionRequest")]
    public async Task<IActionResult> RequestAdoption([FromBody] RequestAdoptionDto dto)
    {
        try
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userIdStr!);

            var user = await userRepository.GetUserByIdAsync(userId);
            var animal = await animalRepository.GetAnimalByIdAsync(dto.AnimalId);

            var userName = user?.FullName ?? $"Користувач #{userId}";
            var animalName = animal?.Name ?? $"Тварина #{dto.AnimalId}";

            var result = await adoptAnimalService.AdoptAnimalAsync(dto.AnimalId, userId);

            await alertService.CreateAsync(new SystemAlertDto
            {
                Message = $"Новий запит на приручення: {animalName} → {userName}",
                Type = "adoption",
                Severity = "info",
                IsAuto = true
            });

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

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