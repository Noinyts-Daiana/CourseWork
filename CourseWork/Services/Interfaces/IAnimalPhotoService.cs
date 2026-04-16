using CourseWork.DTOs;

public interface IAnimalPhotoService
{
    Task<AnimalPhotoDto> UploadPhotoAsync(AnimalPhotoDto dto);
    Task<IEnumerable<AnimalPhotoDto>> GetPhotosByAnimalAsync(int animalId);
    Task DeletePhotoAsync(int id);
    Task SetMainPhotoAsync(int photoId, int animalId);
}