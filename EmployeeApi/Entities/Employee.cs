namespace EmployeeApi.Entities;

public class Employee
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string FIO { get; set; }
    public string? Autobiography { get; set; }
    public DateTime? PromotionDate { get; set; }

    public Guid EmployeePostId { get; set; }
    public EmployeePost? EmployeePost { get; set; }

    public bool IsDeleted { get; set; } = false;
}
