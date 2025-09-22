using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Person.Dto;

public class ValidationErrorResponse
{
    [DataMember(Name = "message")]
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [DataMember(Name = "errors")]
    [JsonPropertyName("errors")]
    public Dictionary<string, string>? Errors { get; set; }

    public ValidationErrorResponse(string? message, Dictionary<string, string>? errors)
    {
        Message = message;
        Errors = errors;
    }
}