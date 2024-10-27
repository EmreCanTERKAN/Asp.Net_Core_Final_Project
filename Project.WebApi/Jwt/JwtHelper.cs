using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Project.WebApi.Jwt
{
    //Nesne üretilmeyeceği için static yapıldı..
    public static class JwtHelper
    {
        public static string GenerateJwtToken(JwtDto jwtDto)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtDto.SecretKey));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtClaimNames.Id,jwtDto.Id.ToString()),
                new Claim(JwtClaimNames.Email,jwtDto.Email),
                new Claim(JwtClaimNames.UserType,jwtDto.UserType.ToString()),
                new Claim(ClaimTypes.Role,jwtDto.UserType.ToString())
            };
            var expireTime = DateTime.Now.AddMinutes(jwtDto.ExpireMinutes);
            var tokenDescriptor = new JwtSecurityToken(jwtDto.Issuer, jwtDto.Audience, claims, null, expireTime, credentials);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return token;
        }

    }
}
