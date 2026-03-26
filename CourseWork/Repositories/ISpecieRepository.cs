using CourseWork.Models;

namespace CourseWork.Repositories;

public interface ISpecieRepository
{
    Task<Specie?> GetSpecieAsync(int id);
    Task<IEnumerable<Specie>> GetAllSpeciesAsync();
    Task AddSpecieAsync(Specie specie);
    Task UpdateSpecieAsync(Specie specie);
    Task DeleteSpecieAsync(int id);
    
}