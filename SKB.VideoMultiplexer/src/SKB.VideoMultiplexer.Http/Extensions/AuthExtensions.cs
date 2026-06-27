using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace SKB.App.SKB.VideoMultiplexer.Http.Extensions;

/// <summary>
/// Provides Auth Extensions to add authentication and authorisation.
/// </summary>
public static class AuthExtensions
{
	/// <summary>
	/// Adds authentication and authorisation services to the IServiceCollection.
	/// </summary>
	/// <param name="services">Service collection to be extended.</param>
	/// <param name="configuration">Configuration instance.</param>
	/// <returns>Extended service collection.</returns>
	public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
	{
		var oidcSection = configuration.GetSection("Authentication:Schemes:OpenIdConnect");
		var authority = oidcSection["Authority"];
		var clientId = oidcSection["ClientId"];
		var clientSecret = oidcSection["ClientSecret"];
		var requireHttps = oidcSection.GetValue<bool>("RequireHttpsMetadata");

		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = "Combined";
			options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
		})
		.AddPolicyScheme("Combined", "Combined", options =>
		{
			options.ForwardDefaultSelector = context =>
			{
				var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
				if (!string.IsNullOrEmpty(authHeader) &&
				    authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
					return JwtBearerDefaults.AuthenticationScheme;

				return CookieAuthenticationDefaults.AuthenticationScheme;
			};
		})
		.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
		.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
		{
			options.Authority = authority;
			options.RequireHttpsMetadata = requireHttps;
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateAudience = false,
				ValidateLifetime = true
			};
			options.MapInboundClaims = false;
		})
		.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
		{
			options.Authority = authority;
			options.ClientId = clientId;
			options.ClientSecret = clientSecret;
			options.RequireHttpsMetadata = requireHttps;
			options.ResponseType = "code";
			options.SaveTokens = true;
			options.GetClaimsFromUserInfoEndpoint = true;
		});

		services.AddAuthorization();
		return services;
	}
}
