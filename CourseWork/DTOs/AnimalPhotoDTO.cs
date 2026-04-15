using Microsoft.AspNetCore.Http;

namespace CourseWork.DTOs;

public class AnimalPhotoDto
{
    public int Id { get; set; }
    public int AnimalId { get; set; }
    
    public IFormFile? File { get; set; } 
    
    public string? FileUrl { get; set; } 
    
    public bool IsMain { get; set; }
}