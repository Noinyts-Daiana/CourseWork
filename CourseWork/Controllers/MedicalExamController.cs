using CourseWork.DTOs;
using CourseWork.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Controllers;

[ApiController]
[Route("api/medical-exams")]
public class MedicalExamController(IMedicalExamService medicalExamService) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetMedicalExams(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 9,
        [FromQuery] string? searchTerm = null)
    {
        var medicalExams = await medicalExamService.GetMedicalExamsAsync(pageNumber, pageSize, searchTerm);

        return Ok(new
        {
            items = medicalExams,
            totalCount = await medicalExamService.GetMedicalExamsCountAsync(searchTerm),
            pageNumber = pageNumber,
            pageSize = pageSize
        });
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetMedicalExamAsync(int id)
    {
        var medicalExam = await medicalExamService.GetMedicalExamAsync(id);
        return Ok(medicalExam);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMedicalExamAsync([FromBody] MedicalExamDTO medicalExamDto)
    {
        await medicalExamService.AddMedicalExamAsync(medicalExamDto);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMedicalExamAsync(int id, [FromBody] MedicalExamDTO medicalExamDto)
    {
        await medicalExamService.UpdateMedicalExamAsync(id, medicalExamDto);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMedicalExamAsync(int id)
    {
        await medicalExamService.DeleteMedicalExamAsync(id);
        return Ok();
    }

}