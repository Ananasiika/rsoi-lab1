namespace Person.Core.Models;

public class Person
{
    /// <summary>
    /// Идентификатор человека.
    /// </summary>
    public int Id { get; }
    
    /// <summary>
    /// Имя.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Возраст.
    /// </summary>
    public int? Age { get; }
    
    /// <summary>
    /// Адрес.
    /// </summary>
    public string? Address { get; }
    
    /// <summary>
    /// Место работы.
    /// </summary>
    public string? Work { get; }

    public Person(int id, string name, int? age, string? address, string? work)
    {
        Id = id;
        Name = name;
        Age = age;
        Address = address;
        Work = work;
    }
}