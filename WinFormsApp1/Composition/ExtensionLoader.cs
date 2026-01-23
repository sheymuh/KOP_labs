using PluginsContracts;
using System.Reflection;

namespace ComponentOprientedApp.Composition;

internal sealed class ExtensionLoader
{
    private readonly string _extensionPath;

    public ExtensionLoader(string extensionPath)
    {
        _extensionPath = Path.GetFullPath(extensionPath);
        Directory.CreateDirectory(_extensionPath);
    }

    public IReadOnlyList<IReportDocumentContract> LoadAll()
    {
        var result = new List<IReportDocumentContract>();

        foreach (var dll in Directory.EnumerateFiles(_extensionPath, "*.dll"))
        {
            try
            {
                var assembly = Assembly.LoadFrom(dll);
                var types = assembly
                    .GetTypes()
                    .Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(IReportDocumentContract)));

                foreach (var type in types)
                {
                    try
                    {
                        if (Activator.CreateInstance(type) is IReportDocumentContract instance)
                        {
                            result.Add(instance);
                        }
                    }
                    catch (Exception ex1) { }
                }
            }
            catch (Exception ex2) { }
        }

        return result;
    }
}
