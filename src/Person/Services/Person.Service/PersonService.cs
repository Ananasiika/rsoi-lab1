using Microsoft.Extensions.Logging;
using Person.Core.Exceptions;
using Person.Core.Interfaces;
using CorePerson = Person.Core.Models.Person;

namespace Person.Service;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;
    private readonly ILogger<PersonService> _logger;

    public PersonService(IPersonRepository personRepository, ILogger<PersonService> logger)
    {
        _personRepository = personRepository;
        _logger = logger;
    }

    public async Task<CorePerson> CreatePersonAsync(string name, int? age, string? address, string? work)
    {
        _logger.LogDebug("Creating person with name: {Name}", name);
        
        try
        {
            var person = await _personRepository.CreatePersonAsync(name, age, address, work);
            _logger.LogInformation("Successfully created person with id {Id}", person.Id);
            return person;
        }
        catch (PersonAlreadyExistsException ex)
        {
            _logger.LogWarning("Failed to create person: {ErrorMessage}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating person with name: {Name}", name);
            throw;
        }
    }

    public async Task<CorePerson> UpdatePersonAsync(int id, string name, int? age, string? address, string? work)
    {
        _logger.LogDebug("Updating person with id: {Id}", id);
        
        try
        {
            var person = await _personRepository.UpdatePersonAsync(id, name, age, address, work);
            _logger.LogInformation("Successfully updated person with id {Id}", person.Id);
            return person;
        }
        catch (PersonNotFoundException ex)
        {
            _logger.LogWarning("Person not found during update: {ErrorMessage}", ex.Message);
            throw;
        }
        catch (PersonAlreadyExistsException ex)
        {
            _logger.LogWarning("Person already exists during update: {ErrorMessage}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating person with id: {Id}", id);
            throw;
        }
    }

    public async Task<CorePerson> GetPersonByIdAsync(int id)
    {
        _logger.LogDebug("Getting person with id: {Id}", id);
        
        try
        {
            return await _personRepository.GetPersonByIdAsync(id);
        }
        catch (PersonNotFoundException ex)
        {
            _logger.LogWarning("Person not found: {ErrorMessage}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting person with id: {Id}", id);
            throw;
        }
    }

    public async Task<CorePerson> DeletePersonAsync(int id)
    {
        _logger.LogDebug("Deleting person with id: {Id}", id);
        
        try
        {
            var person = await _personRepository.DeletePersonAsync(id);
            _logger.LogInformation("Successfully deleted person with id {Id}", person.Id);
            return person;
        }
        catch (PersonNotFoundException ex)
        {
            _logger.LogWarning("Person not found during deletion: {ErrorMessage}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting person with id: {Id}", id);
            throw;
        }
    }

    public async Task<List<CorePerson>> GetPeopleAsync()
    {
        _logger.LogDebug("Getting all people");
        
        try
        {
            var people = await _personRepository.GetPeopleAsync();
            _logger.LogInformation("Retrieved {Count} people", people.Count);
            return people;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all people");
            throw;
        }
    }
}