namespace ComponentContract;

internal interface IComponentContract
{
    string Id { get; }

    string Name { get; }

    UserControl Control { get; }
}
