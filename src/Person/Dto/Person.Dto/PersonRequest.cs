using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Person.Dto;

/// <summary>
/// Модель запроса на создание сущности Person.
/// </summary>
public class PersonRequest
{
    /// <summary>
    /// Имя.
    /// </summary>
    [Required]
    [DataMember(Name = "name")]
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Возраст.
    /// </summary>
    [DataMember(Name = "age")]
    [JsonPropertyName("age")]
    public int? Age { get; set; }

    /// <summary>
    /// Адрес.
    /// </summary>
    [DataMember(Name = "address")]
    [JsonPropertyName("address")]
    public string? Address { get; set; }

    /// <summary>
    /// Место работы.
    /// </summary>
    [DataMember(Name = "work")]
    [JsonPropertyName("work")]
    public string? Work { get; set; }

    // Конструктор для удобства
    public PersonRequest(string name, int? age, string? address, string? work)
    {
        Name = name;
        Age = age;
        Address = address;
        Work = work;
    }
}