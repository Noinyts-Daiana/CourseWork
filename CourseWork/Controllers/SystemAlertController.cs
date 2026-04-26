using CourseWork.DTOs;
using CourseWork.Services;
using CourseWork.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Controllers;

[ApiController]
[Route("api/alerts")]
[Authorize]
public class SystemAlertController(ISystemAlertService alertService) : ControllerBase
{
    [HttpGet]
    [RequirePermission("ViewAlerts")]
    public async Task<IActionResult> GetAll(
        [FromQuery] bool? isDone = false,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20)
    {
        var items = await alertService.GetAllAsync(isDone, pageNumber, pageSize);
        var total = await alertService.GetCountAsync(isDone);
        return Ok(new { items, total, pageNumber, pageSize });
    }

    [HttpGet("{id:int}")]
    [RequirePermission("ViewAlerts")]
    public async Task<IActionResult> GetById(int id)
    {
        var alert = await alertService.GetByIdAsync(id);
        if (alert is null) return NotFound();
        return Ok(alert);
    }

    [HttpPost]
    [RequirePermission("ViewAlerts")]
    public async Task<IActionResult> Create([FromBody] SystemAlertDto dto)
    {
        var created = await alertService.CreateAsync(dto);
        return Ok(created);
    }

    [HttpPatch("{id:int}/done")]
    [RequirePermission("CloseAlerts")]
    public async Task<IActionResult> MarkDone(int id)
    {
        await alertService.MarkDoneAsync(id);
        return Ok(new { message = "Сповіщення закрито" });
    }

    [HttpDelete("{id:int}")]
    [RequirePermission("CloseAlerts")]
    public async Task<IActionResult> Delete(int id)
    {
        await alertService.DeleteAsync(id);
        return NoContent();
    }
}