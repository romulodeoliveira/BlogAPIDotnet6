using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BlogAPIDotnet6.Models;
using Microsoft.IdentityModel.Tokens;

namespace BlogAPIDotnet6.Helper;

public class TokenHelper
{
    // https://jwt.io/
    
    public string Key = "uBMbQErWn&hP7mW5TKa9WisaxTU4EgvG$ZWoFj92Te#NvQmB!Xag$Emzb&eQc7zSkVxWkkb7G&dt9GNpENx7HJfsb*&DHyQvkJW6*e*fvDkrHPT@cem49@*&HRKLGp$e";
    public string CreateToken(UserModel user, IConfiguration configuration)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, "Admin") // EXEMPLO DE COMO USAR ROLES!!! 
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Key));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}