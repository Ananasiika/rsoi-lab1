using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Person.Dto;

/// <summary>
/// Модель ответа с данными Person.
/// </summary>
public class PersonResponse
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    [Required]
    [DataMember(Name = "id")]
    [JsonPropertyName("id")]
    public int Id { get; set; }

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

    public PersonResponse(int id, string name, int? age, string? address, string? work)
    {
        Id = id;
        Name = name;
        Age = age;
        Address = address;
        Work = work;
    }
}