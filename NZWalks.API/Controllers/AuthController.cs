using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository) : ControllerBase
{
    //Register
    //POST: /api/auth/register
    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        var identityUser = new IdentityUser
        {
            UserName = registerRequestDto.UserName,
            Email = registerRequestDto.UserName,
        };
        var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);
        
        // Check if user creation failed
        if (!identityResult.Succeeded)
        {
            return BadRequest(new { errors = identityResult.Errors.Select(e => e.Description).ToArray() });
        }
        
        // User created successfully, now add roles if provided
        if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
        {
            identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
            if (!identityResult.Succeeded)
            {
                return BadRequest(new { errors = identityResult.Errors.Select(e => e.Description).ToArray() });
            }
        }
        return Ok("User created successfully");
    }
    
    //Login
    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var user = await userManager.FindByEmailAsync(loginRequestDto.UserName);
        if (user != null)
        {
            var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (checkPasswordResult)
            {
                //Get roles
                var roles = await userManager.GetRolesAsync(user);
                if (roles != null)
                {
                    //Create a token
                    var jwtToken = tokenRepository.CreateJwtToken(user, roles.ToList());
                    var response = new LoginResponseDto()
                    {
                        JwtToken = jwtToken
                    };
                    return Ok(response);
                }
            }
        }
        return BadRequest("Username or password is incorrect");
    }
}