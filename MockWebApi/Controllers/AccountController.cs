using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtConfiguration _jwtConfiguration;

    public AccountController(IUnitOfWork unitOfWork, IOptions<JwtConfiguration> jwtConfiguration)
    {
        _unitOfWork = unitOfWork;
        _jwtConfiguration = jwtConfiguration.Value;
    }

    [AllowAnonymous]
    [HttpGet("login")]
    public async Task<IActionResult> LoginAsync([FromQuery] string username, [FromQuery] string password)
    {
        var userEntity = await _unitOfWork.UserRepository.FindUser(username);
        var isCorrectCombination = await _unitOfWork.UserRepository.CanUserLoggedIn(userEntity, password);
        if (isCorrectCombination && userEntity != null)
        {
            var token = JwtHelper.GenerateTokenFromUser(username, _jwtConfiguration);
            var refreshToken = StringHelper.RandomString(10);
            userEntity.RefreshToken = refreshToken;
            userEntity.RefreshTokenExpiredIn = DateTime.Now.AddDays(30);
            await _unitOfWork.SaveChangesAsync();

            var result = new TokenDto()
            {
                Token = token,
                RefreshToken = userEntity.RefreshToken,
                RefreshTokenExpiredIn = userEntity.RefreshTokenExpiredIn.Value
            };

            return Ok(result);
        }

        return BadRequest();
    }

    [AllowAnonymous]
    [HttpGet("RefreshToken")]
    public async Task<IActionResult> RefreshTokenAsync([FromQuery] string token, [FromQuery] string refreshToken)
    {
        var username = JwtHelper.GetUsernameInToken(token);
        if (username == null)
        {
            return BadRequest();
        }

        var userEntity = await _unitOfWork.UserRepository.FindUser(username);
        if (userEntity == null || userEntity.RefreshToken != refreshToken ||
            (userEntity.RefreshTokenExpiredIn.HasValue && userEntity.RefreshTokenExpiredIn.Value < DateTime.Now))
        {
            return BadRequest();
        }

        var newToken = JwtHelper.GenerateTokenFromUser(username, _jwtConfiguration);
        userEntity.RefreshTokenExpiredIn = DateTime.Now.AddDays(30);
        await _unitOfWork.SaveChangesAsync();

        var result = new TokenDto()
        {
            Token = newToken,
            RefreshToken = userEntity.RefreshToken,
            RefreshTokenExpiredIn = userEntity.RefreshTokenExpiredIn.Value
        };

        return Ok(result);
    }
}