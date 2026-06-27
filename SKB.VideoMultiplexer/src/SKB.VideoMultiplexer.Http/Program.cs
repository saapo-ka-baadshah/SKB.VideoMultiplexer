using JetBrains.Annotations;
using Microsoft.OpenApi.Models;
using SKB.App.SKB.VideoMultiplexer.Http.Extensions;
using SKB.Core.Hosting.Extensions.WebApi;

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
        builder.Services.AddEndpointsApiExplorer();

        // Add Authentication
        builder.Services.AddAuth(builder.Configuration);

        // Configure SwaggerGen by swashbuckle
        builder.Services.AddSwaggerGenWithAuth(builder.Configuration);

        // Run the hosted application
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
	        app.UseSwagger();
	        app.UseSwaggerUI(options =>
	        {
		        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
	        });
        }

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.AddEndpoints<Program>();
        app.Run();
    }
}
