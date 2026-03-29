using CourseWork.DTOs;

namespace CourseWork.Services;

public interface ICharacteristicService
{
    Task<IEnumerable<CharacteristicDto>> GetCharacteristicsAsync();
    Task<CharacteristicDto> GetCharacteristicAsync(int characteristicId);
    Task AddCharacteristicAsync(CharacteristicDto characteristic);
    Task UpdateCharacteristicAsync(CharacteristicDto characteristic);
    Task DeleteCharacteristicAsync(int characteristicId);
}