// ─── AnimalPhotoController.cs ────────────────────────────────────────────────
using CourseWork.DTOs;
using CourseWork.Services.Interfaces;
using CourseWork.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Controllers;

[ApiController]
[Route("api/animal-photo")]
//[Authorize]
public class AnimalPhotoController(IAnimalPhotoService photoService) : ControllerBase
{
    [HttpPost]
    [RequirePermission("AddAnimal")]
    public async Task<IActionResult> UploadPhoto([FromForm] AnimalPhotoDto dto)
    {
        try
        {
            var result = await photoService.UploadPhotoAsync(dto);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("animal/{animalId}")]
    //[RequirePermission("ViewAnimals")]
    public async Task<IActionResult> GetPhotos(int animalId)
    {
        var photos = await photoService.GetPhotosByAnimalAsync(animalId);
        return Ok(photos);
    }

    [HttpPatch("{id}/setmain/animal/{animalId}")]
    [RequirePermission("EditAnimal")]
    public async Task<IActionResult> SetMainPhoto(int id, int animalId)
    {
        try
        {
            await photoService.SetMainPhotoAsync(id, animalId);
            return Ok(new { message = "Головне фото оновлено" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [RequirePermission("EditAnimal")]
    public async Task<IActionResult> DeletePhoto(int id)
    {
        try
        {
            await photoService.DeletePhotoAsync(id);
            return Ok(new { message = "Фото видалено" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}