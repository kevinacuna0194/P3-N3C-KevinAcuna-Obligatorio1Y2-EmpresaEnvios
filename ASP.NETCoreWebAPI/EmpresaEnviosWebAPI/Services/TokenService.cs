using EmpresaEnviosWebAPI.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmpresaEnviosWebAPI.Services
{
    public class TokenService
    {
        private readonly string SecretKey;
        private readonly string Issuer;
        private readonly string Audience;

        public TokenService(IConfiguration configuration)
        {
            SecretKey = configuration["Jwt:SecretKey"] ?? throw new ArgumentNullException("SecretKey no está configurado");
            Issuer = configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Issuer no está configurado");
            Audience = configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Audience no está configurado");
        }

        public string GenerarToken(JwtDTO jwtDto)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Email, jwtDto.Email),
                new Claim(ClaimTypes.Role, jwtDto.Rol)
            }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = Issuer,
                Audience = Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}
