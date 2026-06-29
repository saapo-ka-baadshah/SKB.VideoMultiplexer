using Microsoft.AspNetCore.Authentication;
using Yarp.ReverseProxy.Transforms;

namespace SKB.App.SKB.VideoMultiplexer.ApiGateway.Extensions;

/// <summary>
/// Extensions in regard to the Reverse Proxy operations.
/// </summary>
public static class ReverseProxyExtensions
{
	/// <summary>
	/// Adds the YARP(Yet Another Reverse Proxy) service to the service collection.
	/// </summary>
	/// <param name="services">Service collection to be extended.</param>
	/// <param name="configuration">Configuration injected.</param>
	/// <returns>Extended service collection.</returns>
	public static IServiceCollection AddYarpService(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddReverseProxy()
			.LoadFromConfig(configuration.GetSection("ReverseProxy"))
			.AddTransforms(context =>
			{
				context.AddRequestTransform(async transformContext =>
				{
					// Get the auth result
					var authResult = await transformContext.HttpContext.AuthenticateAsync();
					if (authResult.Succeeded)
					{
						var accessToken = authResult.Properties.GetTokenValue("access_token");
						if (!string.IsNullOrEmpty(accessToken))
						{
							transformContext.ProxyRequest.Headers.Add("Authorization", $"Bearer {accessToken}");
						}
					}

					// Remove cookie header for CSRF protection
					transformContext.ProxyRequest.Headers.Remove("Cookie");
				});
			});
		return services;
	}
}
