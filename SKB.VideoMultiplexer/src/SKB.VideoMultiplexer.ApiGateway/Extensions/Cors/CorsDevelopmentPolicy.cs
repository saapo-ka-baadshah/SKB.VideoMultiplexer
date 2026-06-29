using Microsoft.AspNetCore.Cors.Infrastructure;

namespace SKB.App.SKB.VideoMultiplexer.ApiGateway.Extensions.Cors;

/// <summary>
/// Cors development policy.
/// </summary>
public static class CorsDevelopmentPolicy
{
    private const string Policy = CorsStringConstants.DevelopmentPolicyName;

    /// <summary>
    /// Adds the development policy to the cors policy stack.
    /// </summary>
    /// <param name="options"><see cref="CorsOptions"/> to be extended.</param>
    /// <returns>Extended <see cref="CorsOptions"/>.</returns>
    public static CorsOptions AddDevelopmentPolicy(this CorsOptions options)
    {
        options.AddPolicy(Policy, builder =>
        {
            builder
                .WithOrigins("http://localhost:3000", "http://localhost:5173")
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
        return options;
    }
}
