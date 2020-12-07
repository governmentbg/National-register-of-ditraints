using Microsoft.IdentityModel.Tokens;
using NRZ.Models.Identity;
using NRZ.Models.Settings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NRZ.Web.Auth
{
    public class JWTManager
    {
        public static JwtSecurityToken GenerateToken(ApplicationUser user, IEnumerable<string> roles, TokenConfig _tokenConfig)
        {
            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.NameIdentifier, user.Id),
                                new Claim(ClaimTypes.Name, user.UserName)
                            };

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var signingKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfig.Secret));

            var token = new JwtSecurityToken
            (
                issuer: _tokenConfig.Issuer,
                audience: _tokenConfig.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(_tokenConfig.ExpirationHours),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
    }
}
