using System.Reflection;

namespace SKB.App.SKB.VideoMultiplexer.Http.Extensions;

/// <summary>
/// Extends the Builder Extensions for Configuration
/// </summary>
public static class ConfigurationExetensions
{
    /// <summary>
    /// Adds the Configurations to the builder
    /// </summary>
    /// <param name="builder">calling HostApplicaitonBuilder <seealso cref="IHostApplicationBuilder"/></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddConfigurations(this IHostApplicationBuilder builder)
    {
        // Add Builder Configuration Options
        AddConfigurationsFromJsonFile(
            builder,
            Path.GetFullPath(
                Directory.GetParent(Path.GetFullPath(Assembly.GetExecutingAssembly().Location))!.ToString()
            )
        );
        builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true);
        return builder;
    }

    private static void AddConfigurationsFromJsonFile(IHostApplicationBuilder builder,
        string basePath)
    {
        builder.Configuration.SetBasePath(basePath)
            .AddJsonFile("appsettings.json");

        if (!builder.Environment.IsProduction())
        {
	        string environmentName = builder.Environment.EnvironmentName.ToLower();
            builder.Configuration.SetBasePath(basePath)
                .AddJsonFile($"appsettings.{environmentName}.json");
        }
    }
}
