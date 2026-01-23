namespace WebApi.Entities;

public class EmployeePost(Guid id, string name)
{
    public Guid Id { get; set; } = id;

    public string Name { get; set; } = name;
}
