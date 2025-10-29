using System.ComponentModel;

namespace ComponentContract.Entities;

public sealed class Employee
{
    [Browsable(false)]
    public Guid Id { get; set; } = Guid.NewGuid();

    public required string FIO { get; set; }

    public string? Autobiography { get; set; }

    // Связь с типом подразделения
    public Guid EmployeePostId { get; set; }
    public EmployeePost EmployeePost { get; set; } = null!;

    // Дата повышения квалификации (мог не проходить)
    public DateTime? PromotionDate { get; set; }

    // Для иерархии работников
    public Guid? ParentId { get; set; }
    public Employee? Parent { get; set; }
    public ICollection<Employee> Children { get; set; } = [];
}
