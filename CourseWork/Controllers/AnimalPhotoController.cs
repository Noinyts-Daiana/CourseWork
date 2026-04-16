using CourseWork.DTOs;
using CourseWork.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Controllers;

[ApiController]
[Route("api/animal-photo")]
public class AnimalPhotoController(IAnimalPhotoService photoService) : ControllerBase
{
    [HttpPost]
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
    public async Task<IActionResult> GetPhotos(int animalId)
    {
        var photos = await photoService.GetPhotosByAnimalAsync(animalId);
        return Ok(photos);
    }

    [HttpPatch("{id}/setmain/animal/{animalId}")]
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