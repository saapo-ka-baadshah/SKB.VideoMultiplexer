using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using SKB.App.SKB.VideoMultiplexer.ApiGateway.Extensions.Auth;

namespace SKB.App.SKB.VideoMultiplexer.ApiGateway.Extensions;

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
			options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
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
		.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
		{
			options.Cookie.Name = "__VideoMultiplexer-Auth";
			options.Cookie.HttpOnly = true;
			options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
			options.Cookie.SameSite = SameSiteMode.Strict;
		})
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

		services.AddAuthorization(options => options.AddAuthPolicies());
		return services;
	}
}
