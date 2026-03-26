using Microsoft.AspNetCore.Http; 

namespace CourseWork.ViewModels;

public class AnimalPhotoUploadViewModel
{
    public int AnimalId { get; set; } 
    
    public IFormFile Photo { get; set; } = null!; 
    
    public bool IsMainPhoto { get; set; } = false; 
}