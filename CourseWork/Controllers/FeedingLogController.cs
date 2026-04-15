using CourseWork.DTOs;
using CourseWork.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
}