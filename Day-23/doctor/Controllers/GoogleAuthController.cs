using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using doctor.Services;
using doctor.Models;
using doctor.Interfaces;

namespace doctor.Controllers;

[ApiController]
[Route("api/googleauth")]
public class GoogleAuthController : Controller
{
    private readonly ITokenService _tokenService;

    public GoogleAuthController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpGet("login")]
    public IActionResult GoogleLogin()
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action("GoogleResponse", "GoogleAuth")
        };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("google-response")]
    public async Task<IActionResult> GoogleResponse()
    {
        var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
        if (!result.Succeeded)
        {
            var error = result.Failure?.Message;
            return BadRequest($"Authentication failed: {error}");
        }


        var email = result.Principal.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        var name = result.Principal.Identity?.Name ?? email ?? "GoogleUser";

        var user = new User
        {
            Username = email ?? name,
            Role = "Patient"
        };

        var token = await _tokenService.GenerateToken(user);

        return Ok(new
        {
            Message = "Google login successful",
            Email = email,
            Name = name,
            Token = token
        });
    }
}
