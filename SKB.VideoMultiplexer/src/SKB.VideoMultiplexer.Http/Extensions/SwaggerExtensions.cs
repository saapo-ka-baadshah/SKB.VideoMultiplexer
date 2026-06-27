using Microsoft.OpenApi.Models;

namespace SKB.App.SKB.VideoMultiplexer.Http.Extensions;

/// <summary>
/// Provides Swagger Extensions to add Swagger services and configure Swagger UI.
/// </summary>
public static class SwaggerExtensions
{
	/// <summary>
	/// Adds Swagger generation services to the IServiceCollection with authentication support.
	/// </summary>
	/// <param name="services">Service collection to be extended.</param>
	/// <param name="configuration">The configuration instance.</param>
	/// <returns>Extended service collection.</returns>
	public static IServiceCollection AddSwaggerGenWithAuth(
		this IServiceCollection services, IConfiguration configuration)
	{
		services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc("v1", new OpenApiInfo()
			{
				Title = "SKB.App.SKB.VideoMultiplexer.Http",
				Version = "v1"
			});

			// Define the Bearer Authentication scheme
			options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
			{
				Type = SecuritySchemeType.Http,
				Scheme = "bearer",
				BearerFormat = "JWT",
				Description = "JWT Authorization header using the Bearer scheme."
			});

			// Enforce global security requirements for all API operations.
			options.AddSecurityRequirement(new OpenApiSecurityRequirement()
			{
				{
					new OpenApiSecurityScheme()
					{
						Reference = new OpenApiReference()
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					},
					[]
				}
			});
		});

		return services;
	}
}
