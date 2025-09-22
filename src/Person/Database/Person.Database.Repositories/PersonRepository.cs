using Microsoft.EntityFrameworkCore;
using Person.Core.Exceptions;
using Person.Core.Interfaces;
using Person.Database.Context;
using CorePerson = Person.Core.Models.Person;
using DbPerson = Person.Database.Models.Person;

namespace Person.Database.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly PersonContext _context;

    public PersonRepository(PersonContext context)
    {
        _context = context;
    }

    public async Task<CorePerson> CreatePersonAsync(string name, int? age, string? address, string? work)
    {
        var existingPerson = await _context.Persons
            .FirstOrDefaultAsync(p => p.Name == name);
            
        if (existingPerson != null)
            throw new PersonAlreadyExistsException($"Person with name '{name}' already exists");

        var dbPerson = new DbPerson(name, age, address, work);
        
        await _context.Persons.AddAsync(dbPerson);
        await _context.SaveChangesAsync();
        
        return PersonConverter.Convert(dbPerson);
    }

    public async Task<CorePerson> UpdatePersonAsync(int id, string? name, int? age, string? address, string? work)
    {
        var dbPerson = await GetDbPersonByIdAsync(id);
        
        if (name != dbPerson.Name)
        {
            var personWithSameName = await _context.Persons
                .FirstOrDefaultAsync(p => p.Name == name && p.Id != id);
                
            if (personWithSameName != null)
                throw new PersonAlreadyExistsException($"Person with name '{name}' already exists");
        }
        
        if (name is not null)
            dbPerson.Name = name;
        if (age is not null)
            dbPerson.Age = age.Value;
        if (address is not null)
            dbPerson.Address = address;
        if (work is not null)
            dbPerson.Work = work;
        
        await _context.SaveChangesAsync();
        
        return PersonConverter.Convert(dbPerson);
    }

    public async Task<CorePerson> GetPersonByIdAsync(int id)
    {
        var dbPerson = await GetDbPersonByIdAsync(id);
        return PersonConverter.Convert(dbPerson);
    }

    public async Task<CorePerson> DeletePersonAsync(int id)
    {
        var dbPerson = await GetDbPersonByIdAsync(id);
        
        _context.Persons.Remove(dbPerson);
        await _context.SaveChangesAsync();
        
        return PersonConverter.Convert(dbPerson);
    }

    public async Task<List<CorePerson>> GetPeopleAsync()
    {
        var persons = await _context.Persons.ToListAsync();
        return persons.ConvertAll(PersonConverter.Convert);
    }

    private async Task<DbPerson> GetDbPersonByIdAsync(int id)
    {
        var dbPerson = await _context.Persons.FirstOrDefaultAsync(p => p.Id == id);
        if (dbPerson is null)
            throw new PersonNotFoundException($"Person with id {id} was not found");
        
        return dbPerson;
    }
}