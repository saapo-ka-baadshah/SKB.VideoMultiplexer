using JetBrains.Annotations;
using SKB.App.SKB.VideoMultiplexer.ApiGateway.Extensions;
using SKB.App.SKB.VideoMultiplexer.ApiGateway.Extensions.Cors;

namespace SKB.App.SKB.VideoMultiplexer.ApiGateway;

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
        builder.Services.AddControllers();
        builder.Services.AddCorsExtensions();

        // Add Authentication
        builder.Services.AddAuth(builder.Configuration);

        // Configure SwaggerGen by swashbuckle
        builder.Services.AddSwaggerGenWithAuth(builder.Configuration);

        // Add YARP (Yet Another Reverse Proxy) service
        builder.Services.AddYarpService(builder.Configuration);

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
        app.UseCors(CorsStringConstants.DevelopmentPolicyName);

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.MapReverseProxy();
        app.Run();
    }
}
