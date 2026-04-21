using CourseWork.DTOs;
using CourseWork.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CourseWork.Controllers;

[ApiController]
[Route("api/feeding-log")]
public class FeedingLogController(IFeedingLogService feedingLogService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> FeedAnimal([FromBody] FeedingLogDto dto)
    {
        try
        {
            await feedingLogService.AddFeedingLogAsync(dto);
            return Ok(new { message = "Тварину успішно нагодовано!" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message }); 
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message }); 
        }
    }

    [HttpGet("animal/{animalId}")]
    public async Task<IActionResult> GetAnimalLogs(int animalId)
    {
        var logs = await feedingLogService.GetLogsByAnimalAsync(animalId);
        return Ok(logs);
    }

    [HttpGet("recent")]
    public async Task<IActionResult> GetRecentLogs([FromQuery] int count = 50)
    {
        var logs = await feedingLogService.GetRecentLogsAsync(count);
        return Ok(logs);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFeedingLog(int id, [FromBody] FeedingLogDto dto)
    {
        try
        {
            var updatedLog = await feedingLogService.UpdateFeedingLogAsync(id, dto);
            return Ok(updatedLog);
        }
        catch (KeyNotFoundException ex) 
        {
            return NotFound(new { message = ex.Message });
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx)
        {
            return BadRequest(new { message = pgEx.MessageText });
        }
        catch (Exception ex)
        {
            return BadRequest(new { 
                message = "Помилка при оновленні запису.", 
                details = ex.Message 
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFeedingLog(int id)
    {
        try
        {
            var isDeleted = await feedingLogService.DeleteFeedingLogAsync(id);
            
            if (!isDeleted) 
                return NotFound(new { message = "Запис годування не знайдено." });

            return Ok(new { message = "Запис успішно видалено." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Помилка при видаленні запису.", details = ex.Message });
        }
    }
}