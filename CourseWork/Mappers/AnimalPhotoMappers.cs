using CourseWork.DTOs;
using CourseWork.Models;

namespace CourseWork.Mappers;

public static class AnimalPhotoMappers
{
    public static AnimalPhotoDto ToDto(this AnimalPhoto photo)
    {
        return new AnimalPhotoDto
        {
            Id = photo.Id,
            AnimalId = photo.AnimalId,
            
            FileUrl = photo.FilePath, 
            
            IsMain = photo.IsMain
            
          
        };
    }
    
    public static AnimalPhoto ToEntity(this AnimalPhotoDto dto, string generatedFilePath)
    {
        return new AnimalPhoto
        {
            AnimalId = dto.AnimalId,
            
            FilePath = generatedFilePath, 
            
            IsMain = dto.IsMain,
            CreatedAt = DateTime.UtcNow 
        };
    }
}