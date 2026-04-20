using System.Security.Claims;
using CourseWork.DTOs;
using CourseWork.Services;
using CourseWork.Services.Interfaces; // Перевір свій шлях до інтерфейсів сервісів
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Controllers;

[ApiController]
[Route("api/transactions")]
public class TransactionController(ITransactionService transactionService) : ControllerBase
{
    [HttpGet("journal")]
    public async Task<IActionResult> GetJournal(
        [FromQuery] string? searchTerm = null,
        [FromQuery] DateTime? fromDate = null, 
        
        [FromQuery] DateTime? toDate = null,   
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10)
    {
        var transactions = await transactionService.GetJournalAsync(searchTerm, fromDate, toDate, pageNumber, pageSize);
        var totalCount = await transactionService.GetJournalCountAsync(searchTerm, fromDate, toDate);

        return Ok(new 
        { 
            items = transactions, 
            totalCount, 
            pageNumber, 
            pageSize 
        });
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories(
        [FromQuery] string? searchTerm = null,
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10)
    {
        var categories = await transactionService.GetCategoriesAsync(searchTerm, pageNumber, pageSize);
        var totalCount = await transactionService.GetCategoriesCountAsync(searchTerm);

        return Ok(new 
        { 
            items = categories, 
            totalCount, 
            pageNumber, 
            pageSize 
        });
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] TransactionDto dto)
    {
        try
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized(new { message = "Не вдалося ідентифікувати користувача" });

            dto.UserId = int.Parse(userIdString);

            var created = await transactionService.CreateTransactionAsync(dto);
            return Ok(created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Сталася внутрішня помилка сервера.", details = ex.Message });
        }
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTransaction(int id, [FromBody] TransactionDto dto)
    {
        var updated = await transactionService.UpdateTransactionAsync(id, dto);
        if (updated == null) return NotFound(new { message = "Транзакцію не знайдено" });
        return Ok(updated);
    }
    
}