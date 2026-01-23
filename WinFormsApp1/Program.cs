using Microsoft.Extensions.Configuration;
using ComponentContract;
using ComponentOprientedApp.Composition;
using ComponentOprientedApp.Licensing;
using ComponentOprientedApp.Utils;

namespace ComponentOprientedApp
{
    internal static class Program
    {
        public static IConfiguration Configuration { get; private set; }

        public static AccessLevel CurrentAccessLevel { get; private set; } = AccessLevel.Minimal;

        public static string? ExtensionPath { get; private set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();

            var path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", Configuration["ComponentsPath"] ?? "Components"));

            var licensePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", Configuration["License"] ?? "License"));
            var licenseProvider = new LicenseProvider(licensePath);
            if (!licenseProvider.IsExpired)
            {
                CurrentAccessLevel = licenseProvider.CurrentLevel;
            }

            var extensionPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", Configuration["PluginsPath"] ?? "Plugins"));
            if (extensionPath is not null) ExtensionPath = extensionPath;

            var loader = new ComponentLoader(path, CurrentAccessLevel);
            var components = loader.LoadAll().ToDictionary(c => c.Metadata.Title, c => c);
            var host = new HostServicesImpl(CurrentAccessLevel, Configuration);

            Application.Run(new FormMain(components, host));
        }
    }
}