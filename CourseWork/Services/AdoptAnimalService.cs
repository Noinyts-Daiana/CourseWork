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
        if (animal != null)
        {
            //TODO вставити перевірку на ownerId
            var adoptRecord = await adoptAnimalRepository.GetByIdAsync(animalId);
            if (adoptRecord == null)
            {
                throw new InvalidOperationException("Ця тварина зараз не в притулку, або вже має власника.");
            }

            adoptRecord.OwnerId = ownerId;
            adoptRecord.AdoptDate = date ?? DateTime.UtcNow;
            await adoptAnimalRepository.UpdateAdoptAnimal(adoptRecord);
            return adoptRecord.ToDto();
        }
        else
        {
            throw new ArgumentException($"Тварину з ID {animalId} не знайдено.");
        }
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
            Status = AdoptionStatus.Returned // Одиниця!
        };

        await adoptAnimalRepository.CreateAdoptAnimal(record);

        return record.ToDto(); 
    }
    
    public async Task<AdoptAnimalDto> AdoptAnimalAsync(int animalId, int ownerId)
    {
        // TODO: перевірку через userRepository.IsActive

        var record = new AdoptAnimal
        {
            AnimalId = animalId,
            OwnerId = ownerId,
            AdoptDate = DateTime.UtcNow,
            Status = AdoptionStatus.Adopted // 0
        };

        await adoptAnimalRepository.CreateAdoptAnimal(record); 
        return record.ToDto();
    }
    
 
}