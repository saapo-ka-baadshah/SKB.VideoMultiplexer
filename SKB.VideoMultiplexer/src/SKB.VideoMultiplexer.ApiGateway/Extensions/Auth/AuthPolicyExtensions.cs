using Microsoft.AspNetCore.Authorization;

namespace SKB.App.SKB.VideoMultiplexer.ApiGateway.Extensions.Auth;

/// <summary>
/// Extensions in regard to the authorization policies.
/// </summary>
public static class AuthPolicyExtensions
{
	/// <summary>
	/// Adds the authorization policies to the <see cref="AuthorizationOptions"/>.
	/// </summary>
	/// <param name="options">Authorization Options to be extended.</param>
	/// <returns>Extended authorization options.</returns>
	public static AuthorizationOptions AddAuthPolicies(this AuthorizationOptions options)
	{
		options.AddPolicy("VideoConferencingPolicy",
			policy =>
			{
				policy.RequireAuthenticatedUser();
				policy.RequireClaim("video-room-creation", true.ToString());
			});
		return options;
	}
}
