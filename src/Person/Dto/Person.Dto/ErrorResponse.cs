using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Person.Dto;

public class ErrorResponse
{
    /// <summary>
    /// Описание ошибки.
    /// </summary>
    [Required]
    [DataMember(Name = "message")]
    [JsonPropertyName("message")]
    public string Message { get; set; }

    public ErrorResponse(string message)
    {
        Message = message;
    }
}