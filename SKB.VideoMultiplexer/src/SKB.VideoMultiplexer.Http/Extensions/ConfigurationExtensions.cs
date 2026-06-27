using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SKB.App.SKB.VideoMultiplexer.Console.Extensions;

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
        return builder;
    }

    private static IHostApplicationBuilder AddConfigurationsFromJsonFile(IHostApplicationBuilder builder,
        string basePath)
    {
        builder.Configuration.SetBasePath(basePath)
            .AddJsonFile("appsettings.json");

        if (!builder.Environment.IsProduction())
        {
            builder.Configuration.SetBasePath(basePath)
                .AddJsonFile($"appsettings.{builder.Environment}.json");
        }

        return builder;
    }
}