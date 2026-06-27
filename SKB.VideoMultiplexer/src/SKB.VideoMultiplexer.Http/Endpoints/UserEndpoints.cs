using SKB.Core.Abstractions.WebApi;

namespace SKB.App.SKB.VideoMultiplexer.Http.Endpoints;

/// <summary>
/// User handling endpoints.
/// </summary>
public class UserEndpoints: IBaseEndpoint
{
	/// <inheritdoc />
	public void RegisterAllMethods(IEndpointRouteBuilder app)
	{
		app.MapGet("/users", () => "Get all users")
			.RequireAuthorization();
	}
}
