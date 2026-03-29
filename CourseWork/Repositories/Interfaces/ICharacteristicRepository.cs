using CourseWork.Models;

namespace CourseWork.Repositories;

public interface ICharacteristicRepository
{
  Task<IEnumerable<Characteristic>> GetCharacteristicsAsync(); 
  Task<Characteristic?> GetCharacteristicsByIdAsync(int id); 
  Task AddCharacteristicAsync(Characteristic characteristic); 
  Task DeleteCharacteristicAsync(int characteristicId); 
  Task UpdateCharacteristicAsync(Characteristic characteristic);
}