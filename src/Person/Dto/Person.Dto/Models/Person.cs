using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Person.Dto.Models;

public class Person
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    [Required]
    [DataMember(Name = "id")]
    public int Id { get; set; }
    
    /// <summary>
    /// Имя.
    /// </summary>
    [Required]
    [DataMember(Name = "name")]
    public string Name { get; set; }
    
    /// <summary>
    /// Возраст.
    /// </summary>
    [DataMember(Name = "age")]
    public int? Age { get; set; }
    
    /// <summary>
    /// Адрес.
    /// </summary>
    [DataMember(Name = "address")]
    public string? Address { get; set; }
    
    /// <summary>
    /// Место работы.
    /// </summary>
    [DataMember(Name = "work")]
    public string? Work { get; set; }

    public Person(int id, string name, int? age, string? address, string? work)
    {
        Id = id;
        Name = name;
        Age = age;
        Address = address;
        Work = work;
    }
}