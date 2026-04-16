using CourseWork.DTOs;
using CourseWork.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Controllers;

[ApiController]
[Route("api/food-type")]
public class FoodTypeController(IFoodTypeService foodTypeService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetFoodTypes(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 9,
        [FromQuery] string? searchTerm = null)
    {
        var items = await foodTypeService.GetAllAsync(pageNumber, pageSize, searchTerm);
        var totalCount = await foodTypeService.GetCountAsync(searchTerm);

        return Ok(new
        {
            items,
            totalCount,
            pageNumber,
            pageSize
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var foodType = await foodTypeService.GetByIdAsync(id);
        if (foodType == null)
        {
            return NotFound(new { message = $"Тип корму з ID {id} не знайдено" });
        }

        return Ok(foodType);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] FoodTypeDto foodTypeDto)
    {
        await foodTypeService.AddAsync(foodTypeDto);
        return Ok(new { message = "Тип корму успішно додано" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] FoodTypeDto foodTypeDto)
    {
        try
        {
            await foodTypeService.UpdateAsync(id, foodTypeDto);
            return Ok(new { message = "Дані про корм оновлено" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await foodTypeService.DeleteAsync(id);
            return Ok(new { message = "Запис видалено" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
    
    [HttpPatch("{id}/adjust-stock")]
    public async Task<IActionResult> AdjustStock(int id, [FromBody] FoodTypeDto dto)
    {
        try
        {
            await foodTypeService.AdjustStockAsync(id, dto.Amount);
            return Ok(new { message = "Залишок успішно оновлено" });
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
    [HttpGet("brands")]
    public async Task<IActionResult> GetBrands(
        [FromQuery] string? searchTerm = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var brands = await foodTypeService.GetBrandsAsync(searchTerm, pageNumber, pageSize);
        var totalCount = await foodTypeService.GetBrandsCountAsync(searchTerm);

        return Ok(new
        {
            items = brands, 
            totalCount,
            pageNumber,
            pageSize
        });
    }
}