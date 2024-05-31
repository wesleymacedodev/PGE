using PGE.Models;

namespace PGE.Interfaces
{
    public interface IAuthenticate
    {
        Task<bool> AuthenticateAsync(string nome, string senha);
        Task<bool> UserExists(string nome);
        public string GenerateToken(int id, string nome);
        
        public Task<Login> GetUserByName(string nome);
    }
}
