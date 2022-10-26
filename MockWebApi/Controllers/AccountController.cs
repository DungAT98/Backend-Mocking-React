using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mock.Application.Helpers;
using Mock.Application.Repositories;
using Mock.Domain.Configurations;
using Mock.Domain.Dto;

namespace MockWebApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly JwtConfiguration _jwtConfiguration;

    public AccountController(IUserRepository userRepository, IOptions<JwtConfiguration> jwtConfiguration)
    {
        _userRepository = userRepository;
        _jwtConfiguration = jwtConfiguration.Value;
    }

    [AllowAnonymous]
    [HttpGet("login")]
    public async Task<IActionResult> LoginAsync([FromQuery] string username, [FromQuery] string password)
    {
        var isCorrectCombination = await _userRepository.CanUserLoggedIn(username, password);
        if (isCorrectCombination)
        {
            var token = JwtHelper.GenerateTokenFromUser(username, _jwtConfiguration);
            var result = new TokenDto()
            {
                Token = token,
                ExpiredIn = DateTime.UtcNow.AddMinutes(5)
            };
            
            return Ok(result);
        }

        return BadRequest();
    }
}