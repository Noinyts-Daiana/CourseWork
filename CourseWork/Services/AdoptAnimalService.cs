using CourseWork.DTOs;
using CourseWork.Mappers;
using CourseWork.Models;
using CourseWork.Repositories;

namespace CourseWork.Services;

public class AdoptAnimalService(IAdoptAnimalRepository adoptAnimalRepository, IAnimalRepository animalRepository): IAdoptAnimalService
{
    public async Task<AdoptAnimalDto> RegisterArrivalAsync(int animalId, DateTime? date = null)
    {
        var animal = await animalRepository.GetAnimalByIdAsync(animalId);
        if (animal != null)
        {
            var arrivalDate = date ?? DateTime.UtcNow;
            
            var adoptRecord = new AdoptAnimalDto()
            {
                AnimalId = animalId,
                ArrivalDate = arrivalDate
            };

            var adoptRecordModel = adoptRecord.ToEntity();
            await adoptAnimalRepository.CreateAdoptAnimal(adoptRecordModel);
            return new AdoptAnimalDto
            {
                Id = adoptRecord.Id,
                AnimalId = adoptRecord.AnimalId,
                ArrivalDate = adoptRecord.ArrivalDate
            };
        }
        else
        {
            throw new ArgumentException($"Тварину з ID {animalId} не знайдено.");
        }
    }

    public async Task<AdoptAnimalDto> AdoptAnimalAsync(int animalId, int ownerId, DateTime? date = null)
    {
        var animal = await animalRepository.GetAnimalByIdAsync(animalId);
        if (animal == null) 
            throw new KeyNotFoundException($"Тварину з ID {animalId} не знайдено.");

        var currentRecord = await adoptAnimalRepository.GetByIdAsync(animalId); 
        
        if (currentRecord != null)
        {
            if (currentRecord.OwnerId != null || currentRecord.Status == AdoptionStatus.Adopted) 
            {
                throw new InvalidOperationException("Ця тваринка вже знайшла свій дім!");
            }
        }

        var adoptRecord = new AdoptAnimal
        {
            AnimalId = animalId,
            OwnerId = ownerId,
            AdoptDate = date ?? DateTime.UtcNow,
            Status = AdoptionStatus.Adopted 
        };

        await adoptAnimalRepository.CreateAdoptAnimal(adoptRecord); 
        return adoptRecord.ToDto();
    }

    public async Task<IEnumerable<AdoptAnimalDto>> GetAvailableAnimalsAsync()
    {
        var availableAnimals = await adoptAnimalRepository.GetAvailableAnimalsAsync();
        return availableAnimals.Select(a => a.ToDto());
    }

    public async Task<IEnumerable<AdoptAnimalDto>> GetUserAdoptionsAsync(int ownerId)
    {
        var adoptions = await adoptAnimalRepository.GetByUserIdAsync(ownerId);

        return adoptions.Select(aa => new AdoptAnimalDto
        {
            Id = aa.Id,
            AnimalId = aa.AnimalId,
            AnimalName = aa.Animal?.Name ?? "Невідома тварина",
            Status = (int)aa.Status, 
            ArrivalDate = aa.ArrivalDate,
            AdoptDate = aa.AdoptDate
        });
    }
    
    public async Task<AdoptAnimalDto> RegisterArrivalAsync(int animalId, int? ownerId = null)
    {
        var animal = await animalRepository.GetAnimalByIdAsync(animalId);
        if (animal == null) throw new ArgumentException("Тварину не знайдено");

        var record = new AdoptAnimal
        {
            AnimalId = animalId,
            OwnerId = ownerId,
            ArrivalDate = DateTime.UtcNow,
            Status = AdoptionStatus.Returned 
        };

        await adoptAnimalRepository.CreateAdoptAnimal(record);

        return record.ToDto(); 
    }
    
    public async Task<AdoptAnimalDto> AdoptAnimalAsync(int animalId, int ownerId)
    {
        var isAlreadyAdopted = await adoptAnimalRepository.IsAnimalAlreadyAdoptedAsync(animalId);
    
        if (isAlreadyAdopted)
        {
            throw new InvalidOperationException("Ця тваринка вже має люблячу родину і не може бути приручена знову!");
        }

        var record = new AdoptAnimal
        {
            AnimalId = animalId,
            OwnerId = ownerId,
            AdoptDate = DateTime.UtcNow,
            Status = AdoptionStatus.Adopted 
        };

        await adoptAnimalRepository.CreateAdoptAnimal(record); 
        return record.ToDto();
    }
    
 
}