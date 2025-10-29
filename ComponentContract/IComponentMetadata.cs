namespace ComponentContract;

public interface IComponentMetadata
{
    string Id { get; }
    string Title { get; }
    ComponentType ComponentType { get; }
    AccessLevel RequiredAccess { get; }
}
