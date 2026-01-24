using System.Text.Json.Serialization;

namespace EmployeeApi.Entities;

public record EmployeeCreateDto(
    [property: JsonPropertyName("fio")] string FIO,
    [property: JsonPropertyName("employee_post_id")] Guid EmployeePostId,
    [property: JsonPropertyName("autobiography")] string? Autobiography,
    [property: JsonPropertyName("promotion_date")] DateTime? PromotionDate
);

public record EmployeeReadDto(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("fio")] string FIO,
    [property: JsonPropertyName("employee_post_id")] Guid EmployeePostId,
    [property: JsonPropertyName("autobiography")] string? Autobiography,
    [property: JsonPropertyName("promotion_date")] DateTime? PromotionDate,
    [property: JsonPropertyName("is_deleted")] bool IsDeleted
);

public record EmployeeUpdateDto(
    [property: JsonPropertyName("fio")] string FIO,
    [property: JsonPropertyName("employee_post_id")] Guid EmployeePostId,
    [property: JsonPropertyName("autobiography")] string? Autobiography,
    [property: JsonPropertyName("promotion_date")] DateTime? PromotionDate
);
