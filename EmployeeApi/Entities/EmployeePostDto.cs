using System.Text.Json.Serialization;

namespace EmployeeApi.Entities;

public record EmployeePostReadDto(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("is_deprecated")] bool IsDeprecated);
