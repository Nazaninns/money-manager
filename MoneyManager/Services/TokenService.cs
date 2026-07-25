using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MoneyManager.Models;
using MoneyManager.Services.Interfaces;

namespace MoneyManager.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateToken(User user)
    {
        //Claims
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.Id.ToString()),
            new (ClaimTypes.Email, user.Email),
        };
    
        //Secret key
        var secretKey = _configuration["JwtSettings:Secret"]
                        ?? throw new InvalidOperationException("Jwt Secret is not configured");
        //Get bytes
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        // Define signing credentials
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        
        //Token config
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(double.Parse(_configuration["JwtSettings:ExpiryInHours"] ?? "1")),
            SigningCredentials = credentials,
            Issuer = _configuration["JwtSettings:Issuer"],
            Audience = _configuration["JwtSettings:Audience"],
        };
        
        //Create and serialization token
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
}