using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;
using SKB.App.SKB.VideoMultiplexer.Console.Extensions;

namespace SKB.App.SKB.VideoMultiplexer.Http;

/// <summary>
/// Public Start-point for the program
/// </summary>
[PublicAPI]
internal class Program
{
    /// <summary>
    /// Main method to call the SKB.VideoMultiplexer Console program
    /// </summary>
    /// <param name="args">arguments pass through</param>
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add Configurations
        builder.AddConfigurations();

        // Run the hosted application

        var app = builder.Build();
        app.Run();
    }
}