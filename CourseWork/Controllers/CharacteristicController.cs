using CourseWork.DTOs;
using CourseWork.Models;
using CourseWork.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Controllers;

[ApiController]
[Route("/api/characteristics")]
public class CharacteristicController(ICharacteristicService characteristicService): ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetAllCharacteristics()
    {
        var characteristicsAsync = await characteristicService.GetCharacteristicsAsync();
        return Ok(characteristicsAsync);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCharacteristic(int id)
    {
        var characteristicsAsync = await characteristicService.GetCharacteristicAsync(id);
        return Ok(characteristicsAsync);
    }

    [HttpPost]
    public async Task AddCharacteristic([FromBody] CharacteristicDto characteristic)
    {
        await characteristicService.AddCharacteristicAsync(characteristic);
    }

    [HttpPut("{id}")]
    public async Task UpdateCharacteristic(int id, [FromBody] CharacteristicDto characteristic)
    {
        await characteristicService.UpdateCharacteristicAsync(characteristic);
    }
    
    [HttpDelete("{id}")]
    public async Task DeleteCharacteristic(int id)
    {
        await characteristicService.DeleteCharacteristicAsync(id);
    }
    
}