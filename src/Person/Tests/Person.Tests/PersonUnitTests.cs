using Microsoft.EntityFrameworkCore;
using Person.Core.Exceptions;
using Person.Core.Interfaces;
using Person.Database.Context;
using Person.Database.Repositories;
using Xunit;

namespace Person.Tests;

public class PersonUnitTests 
{
    private readonly DbContextOptions<PersonContext> _dbContextOptions;
    private readonly PersonContext _context;
    private readonly IPersonRepository _repository;

    public PersonUnitTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<PersonContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new PersonContext(_dbContextOptions);
        _repository = new PersonRepository(_context);
    }

    [Fact]
    public async Task CreatePersonAsync_ShouldCreatePerson_WhenValidData()
    {
        // Arrange
        var name = "John Doe";
        var age = 30;
        var address = "123 Main St";
        var work = "Developer";

        // Act
        var result = await _repository.CreatePersonAsync(name, age, address, work);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(name, result.Name);
        Assert.Equal(age, result.Age);
        Assert.Equal(address, result.Address);
        Assert.Equal(work, result.Work);
        Assert.True(result.Id > 0);

        // Verify in database
        var dbPerson = await _context.Persons.FirstOrDefaultAsync();
        Assert.NotNull(dbPerson);
        Assert.Equal(name, dbPerson.Name);
    }

    [Fact]
    public async Task CreatePersonAsync_ShouldThrowException_WhenPersonWithSameNameExists()
    {
        // Arrange
        var name = "John Doe";
        await _repository.CreatePersonAsync(name, 30, "Address", "Work");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<PersonAlreadyExistsException>(() =>
            _repository.CreatePersonAsync(name, 25, "Another Address", "Another Work"));

        Assert.Contains($"Person with name '{name}' already exists", exception.Message);
    }

    [Fact]
    public async Task GetPersonByIdAsync_ShouldReturnPerson_WhenPersonExists()
    {
        // Arrange
        var createdPerson = await _repository.CreatePersonAsync("John Doe", 30, "Address", "Work");
        var personId = createdPerson.Id;

        // Act
        var result = await _repository.GetPersonByIdAsync(personId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(personId, result.Id);
        Assert.Equal("John Doe", result.Name);
    }

    [Fact]
    public async Task GetPersonByIdAsync_ShouldThrowException_WhenPersonNotFound()
    {
        // Arrange
        var nonExistentId = 999;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<PersonNotFoundException>(() =>
            _repository.GetPersonByIdAsync(nonExistentId));

        Assert.Contains($"Person with id {nonExistentId} was not found", exception.Message);
    }

    [Fact]
    public async Task UpdatePersonAsync_ShouldUpdatePerson_WhenValidData()
    {
        // Arrange
        var createdPerson = await _repository.CreatePersonAsync("John Doe", 30, "Old Address", "Old Work");
        var personId = createdPerson.Id;

        var newName = "Jane Doe";
        var newAge = 35;
        var newAddress = "New Address";
        var newWork = "New Work";

        // Act
        var result = await _repository.UpdatePersonAsync(personId, newName, newAge, newAddress, newWork);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(personId, result.Id);
        Assert.Equal(newName, result.Name);
        Assert.Equal(newAge, result.Age);
        Assert.Equal(newAddress, result.Address);
        Assert.Equal(newWork, result.Work);

        // Verify in database
        var updatedPerson = await _repository.GetPersonByIdAsync(personId);
        Assert.Equal(newName, updatedPerson.Name);
    }

    [Fact]
    public async Task UpdatePersonAsync_ShouldThrowException_WhenPersonNotFound()
    {
        // Arrange
        var nonExistentId = 999;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<PersonNotFoundException>(() =>
            _repository.UpdatePersonAsync(nonExistentId, "New Name", 30, "Address", "Work"));

        Assert.Contains($"Person with id {nonExistentId} was not found", exception.Message);
    }

    [Fact]
    public async Task UpdatePersonAsync_ShouldThrowException_WhenNameConflict()
    {
        // Arrange
        var person1 = await _repository.CreatePersonAsync("John Doe", 30, "Address1", "Work1");
        var person2 = await _repository.CreatePersonAsync("Jane Doe", 25, "Address2", "Work2");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<PersonAlreadyExistsException>(() =>
            _repository.UpdatePersonAsync(person2.Id, "John Doe", 25, "Address2", "Work2"));

        Assert.Contains("Person with name 'John Doe' already exists", exception.Message);
    }

    [Fact]
    public async Task UpdatePersonAsync_ShouldAllowSameName_WhenUpdatingSamePerson()
    {
        // Arrange
        var person = await _repository.CreatePersonAsync("John Doe", 30, "Address", "Work");
        var personId = person.Id;

        // Act - обновляем с тем же именем (должно работать)
        var result = await _repository.UpdatePersonAsync(personId, "John Doe", 31, "New Address", "New Work");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John Doe", result.Name);
        Assert.Equal(31, result.Age);
    }

    [Fact]
    public async Task DeletePersonAsync_ShouldDeletePerson_WhenPersonExists()
    {
        // Arrange
        var person = await _repository.CreatePersonAsync("John Doe", 30, "Address", "Work");
        var personId = person.Id;

        // Act
        var result = await _repository.DeletePersonAsync(personId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(personId, result.Id);

        // Verify person is deleted
        var exception = await Assert.ThrowsAsync<PersonNotFoundException>(() =>
            _repository.GetPersonByIdAsync(personId));

        Assert.Contains($"Person with id {personId} was not found", exception.Message);
    }

    [Fact]
    public async Task DeletePersonAsync_ShouldThrowException_WhenPersonNotFound()
    {
        // Arrange
        var nonExistentId = 999;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<PersonNotFoundException>(() =>
            _repository.DeletePersonAsync(nonExistentId));

        Assert.Contains($"Person with id {nonExistentId} was not found", exception.Message);
    }

    [Fact]
    public async Task GetPeopleAsync_ShouldReturnAllPersons()
    {
        // Arrange
        await _repository.CreatePersonAsync("John Doe", 30, "Address1", "Work1");
        await _repository.CreatePersonAsync("Jane Doe", 25, "Address2", "Work2");

        // Act
        var result = await _repository.GetPeopleAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, p => p.Name == "John Doe");
        Assert.Contains(result, p => p.Name == "Jane Doe");
    }

    [Fact]
    public async Task GetPeopleAsync_ShouldReturnEmptyList_WhenNoPersons()
    {
        // Act
        var result = await _repository.GetPeopleAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task CreatePersonAsync_ShouldHandleNullOptionalFields()
    {
        // Arrange
        var name = "John Doe";
        int? age = null;
        string? address = null;
        string? work = null;

        // Act
        var result = await _repository.CreatePersonAsync(name, age, address, work);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(name, result.Name);
        Assert.Null(result.Age);
        Assert.Null(result.Address);
        Assert.Null(result.Work);
    }

    [Fact]
    public async Task UpdatePersonAsync_ShouldHandleNullOptionalFields()
    {
        // Arrange
        var person = await _repository.CreatePersonAsync("John Doe", 30, "Address", "Work");
        var personId = person.Id;

        // Act
        var result = await _repository.UpdatePersonAsync(personId, "Jane Doe", null, null, null);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Jane Doe", result.Name);
        Assert.Null(result.Address);
        Assert.Null(result.Work);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}