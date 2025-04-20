using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CarTracking.BE.Application.Services;

public interface ITokenService
{
    string GenerateToken(Dictionary<string, string> data);
}

public class TokenService : ITokenService
{
    public string GenerateToken(Dictionary<string, string> data)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key_your_secret_key_your_secret_key"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = [];
        foreach (var claim in data)
        {
            if (!string.IsNullOrEmpty(claim.Value))
            {
                claims.Add(new(claim.Key, claim.Value));
            }
        }

        var token = new JwtSecurityToken(
            issuer: "your_issuer",
            audience: "your_audience",
            claims: claims,
            expires: DateTime.Now.AddDays(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}