using System.Text.Json.Serialization;

namespace WebApi.Entities;

public record EmployeePostDto([property: JsonPropertyName("name")] string Name);
