using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace SKB.App.SKB.VideoMultiplexer.Http.Controllers;

/// <summary>
/// Controller for handling authentication-related actions.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController: ControllerBase
{
	/// <summary>
	/// Handles the login action and redirects to the specified return URL after successful authentication.
	/// </summary>
	/// <param name="returnUrl">Return URL after successful login.</param>
	/// <returns>ActionResult with login information.</returns>
	[HttpGet("login")]
	public IActionResult Login([FromQuery] string returnUrl = "/")
	{
		return Challenge(
			new AuthenticationProperties { RedirectUri = returnUrl },
			OpenIdConnectDefaults.AuthenticationScheme);
	}

	/// <summary>
	/// Handles the logout action and redirects to the specified return URL after successful logout.
	/// </summary>
	/// <param name="returnUrl">Return URL after successful logout.</param>
	/// <returns>ActionResult with logout information.</returns>
	[HttpGet("logout")]
	public IActionResult Logout([FromQuery] string returnUrl = "/")
	{
		return SignOut(
			new AuthenticationProperties { RedirectUri = returnUrl },
			OpenIdConnectDefaults.AuthenticationScheme,
			OpenIdConnectDefaults.AuthenticationScheme);
	}

	/// <summary>
	/// Retrieves the authenticated user's information.
	/// </summary>
	/// <returns>ActionResult with user information.</returns>
	[HttpGet("user")]
	public IActionResult GetUser()
	{
		if (User.Identity?.IsAuthenticated == true)
		{
			return Ok(new
			{
				IsAuthenticated = true,
				Name = User.Identity.Name,
				Claims = User.Claims.Select(c => new { c.Type, c.Value })
			});
		}
		return Ok(new { IsAuthenticated = false });
	}
}
