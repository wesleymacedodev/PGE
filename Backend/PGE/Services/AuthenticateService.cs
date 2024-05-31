using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.ObjectPool;
using Microsoft.IdentityModel.Tokens;
using PGE.Interfaces;
using PGE.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PGE.Services
{
    public class AuthenticateService : IAuthenticate
    {
        private readonly PGEContext _context;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly SymmetricSecurityKey _jwtIssuerKey;

        public AuthenticateService(PGEContext context, IConfiguration configuration)
        {
            _context = context;
            _jwtIssuer = configuration["Settings:JWTIssuer"];
            _jwtAudience = configuration["Settings:JWTAudience"];
            _jwtIssuerKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Settings:JWTIssuerKey"]));
        }

        public async Task<bool> AuthenticateAsync(string nome, string senha)
        {
            var usuario = await _context.Login.Where(x => x.Nome.ToLower() == nome.ToLower()).FirstOrDefaultAsync();
            if (usuario == null)
            {
                return false;
            }
            using var hmac = new HMACSHA512(usuario.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(senha));
            for (int x = 0; x < computedHash.Length; x++)
            {
                if (computedHash[x] != usuario.PasswordHash[x]) return false;
            }
            return true;
        }

        public string GenerateToken(int id, string nome)
        {
            var claims = new[]
            {
                new Claim("id", id.ToString()),
                new Claim("nome", nome),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var privateKey = _jwtIssuerKey;
            var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddMinutes(20);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _jwtIssuer,
                audience: _jwtAudience,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Login> GetUserByName(string nome)
        {
            return await _context.Login.Where(x => x.Nome.ToLower() == nome.ToLower()).FirstOrDefaultAsync();
        }

        public async Task<bool> UserExists(string nome)
        {
            var usuario = await _context.Login.Where(x => x.Nome.ToLower() == nome.ToLower()).FirstOrDefaultAsync();
            if (usuario == null)
            {
                return false;
            }
            return true;
        }
    }
}
