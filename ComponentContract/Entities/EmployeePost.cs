using System.ComponentModel;

namespace ComponentContract.Entities;

public class EmployeePost
{
    [Browsable(false)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [DisplayName("Наименование")]
    public required string Name { get; set; }
}
