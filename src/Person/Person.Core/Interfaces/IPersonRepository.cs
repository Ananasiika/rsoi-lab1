namespace Person.Core.Interfaces;

/// <summary>
/// Интерфейс репозитория для работы с сущностью Person.
/// </summary>
public interface IPersonRepository
{
    public Task<Models.Person> CreatePersonAsync(string name, int? age, string? address, string? work);
    
    public Task<Models.Person> UpdatePersonAsync(int id, string name, int? age, string? address, string? work);
    
    public Task<Models.Person> GetPersonByIdAsync(int id);
    
    public Task<Models.Person> DeletePersonAsync(int id);
    
    public Task<List<Models.Person>> GetPeopleAsync();
}