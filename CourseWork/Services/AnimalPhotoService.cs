using CourseWork.DTOs;
using CourseWork.Mappers; 
using CourseWork.Models;
using CourseWork.Repositories;
using CourseWork.Repositories.Interfaces;
using CourseWork.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace CourseWork.Services;
public class AnimalPhotoService(
    IAnimalPhotoRepository photoRepository,
    IWebHostEnvironment environment) : IAnimalPhotoService
{
    public async Task<AnimalPhotoDto> UploadPhotoAsync(AnimalPhotoDto dto)
    {
        if (dto.File == null || dto.File.Length == 0)
            throw new ArgumentException("Файл не вибрано");

        var uploadsFolder = Path.Combine(environment.WebRootPath, "images", "animals");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = Guid.NewGuid().ToString() + "_" + dto.File.FileName;
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await dto.File.CopyToAsync(fileStream);
        }

        if (dto.IsMain)
        {
            await photoRepository.ClearMainPhotoFlagAsync(dto.AnimalId);
        }

        var photo = new AnimalPhoto
        {
            AnimalId = dto.AnimalId,
            FilePath = $"/images/animals/{uniqueFileName}", 
            IsMain = dto.IsMain,
            CreatedAt = DateTime.UtcNow
        };

        await photoRepository.AddAsync(photo);
        await photoRepository.SaveChangesAsync();

        return photo.ToDto();
    }

    public async Task<IEnumerable<AnimalPhotoDto>> GetPhotosByAnimalAsync(int animalId)
    {
        var photos = await photoRepository.GetByAnimalIdAsync(animalId);
        return photos.Select(p => p.ToDto());
    }

    public async Task DeletePhotoAsync(int id)
    {
        var photo = await photoRepository.GetByIdAsync(id);
        if (photo == null) throw new KeyNotFoundException("Фото не знайдено");

        var physicalPath = Path.Combine(environment.WebRootPath, photo.FilePath.TrimStart('/'));
        if (File.Exists(physicalPath))
        {
            File.Delete(physicalPath);
        }

        await photoRepository.DeleteAsync(photo);
        await photoRepository.SaveChangesAsync();
    }

    public async Task SetMainPhotoAsync(int photoId, int animalId)
    {
        var photo = await photoRepository.GetByIdAsync(photoId);
        if (photo == null || photo.AnimalId != animalId) 
            throw new KeyNotFoundException("Фото не знайдено");

        await photoRepository.ClearMainPhotoFlagAsync(animalId);
        photo.IsMain = true;
        await photoRepository.SaveChangesAsync();
    }
    
    
}