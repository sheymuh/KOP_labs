namespace WebApi.Entities;

public class EmployeePost
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public required string Name { get; set; }

    // Soft delete при удалении типа (упраздненный тип)
    public bool IsDeprecated { get; set; } = false;
}
