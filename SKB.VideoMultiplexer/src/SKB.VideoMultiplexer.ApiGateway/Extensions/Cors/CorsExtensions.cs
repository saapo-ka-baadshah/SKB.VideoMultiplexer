using SKB.App.SKB.VideoMultiplexer.ApiGateway.Extensions.Cors;

namespace SKB.App.SKB.VideoMultiplexer.ApiGateway.Extensions.Cors;

/// <summary>
/// Extensions in regard to the CORS operations.
/// </summary>
public static class CorsExtensions
{
    /// <summary>
    /// Adds the CORS to the service collection.
    /// </summary>
    /// <param name="services">Services to be extended.</param>
    /// <returns>Extended service collection.</returns>
    public static IServiceCollection AddCorsExtensions(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDevelopmentPolicy();
        });
        return services;
    }
}
