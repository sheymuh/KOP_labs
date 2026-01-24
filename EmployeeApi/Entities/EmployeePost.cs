namespace EmployeeApi.Entities;

public class EmployeePost
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    // cant delete, cause of foreign key constraints, so mark as deprecated
    public bool IsDeprecated { get; set; } = false;
}
