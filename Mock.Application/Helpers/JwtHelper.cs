using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Mock.Domain.Configurations;
using Mock.Domain.Models;
using Newtonsoft.Json;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Mock.Application.Helpers;

public static class JwtHelper
{
    public static string GenerateTokenFromUser(string username, JwtConfiguration jwtConfiguration)
    {
        var issuer = jwtConfiguration.Issuer;
        var audience = jwtConfiguration.Audience;
        var key = Encoding.ASCII.GetBytes(jwtConfiguration.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, username)
            }),
            Expires = DateTime.Now.AddSeconds(30),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        return jwtToken;
    }

    public static TokenInformationModel? GetTokenInformation(string token)
    {
        var tokenArgs = token.Split(".", StringSplitOptions.RemoveEmptyEntries);
        if (tokenArgs.Length != 3)
        {
            return null;
        }

        var payload = StringHelper.Base64Decode(tokenArgs[1]);
        var deSerializePayload = JsonConvert.DeserializeObject<TokenInformationModel>(payload);
        
        return deSerializePayload;
    }

    public static string? GetUsernameInToken(string token)
    {
        var tokenInformation = GetTokenInformation(token);
        if (tokenInformation == null)
        {
            return null;
        }

        return tokenInformation.Sub;
    }
}