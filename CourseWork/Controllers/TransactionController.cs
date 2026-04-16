using CourseWork.DTOs;
using CourseWork.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController(ITransactionService transactionService) : ControllerBase
{
    [HttpGet("journal")]
    public async Task<IActionResult> GetJournal()
    {
        var transactions = await transactionService.GetJournalAsync();
        return Ok(transactions);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await transactionService.GetCategoriesAsync();
        return Ok(categories);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] TransactionDto dto)
    {
        var created = await transactionService.CreateTransactionAsync(dto);
        return Ok(created);
    }
}