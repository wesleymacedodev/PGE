using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PGE.Interfaces;
using PGE.Models;

namespace PGE.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly PGEContext _context;

        public LoginRepository(PGEContext context)
        {
            _context = context;
        }

        public void Alterar(Login login)
        {
            _context.Entry(login).State = EntityState.Modified;
        }

        public void Excluir(Login login)
        {
            _context.Login.Remove(login);
        }

        public void Incluir(Login login)
        {
            _context.Login.Add(login);
        }

        public async Task<bool> Salvar()
        {
            return await _context.SaveChangesAsync() > 0; 
        }

        public async Task<Login> SelecionarPelaPK(int id)
        {
            var login = await _context.Login.Where(x => x.Id == id).FirstOrDefaultAsync();
            return login;
        }

        public async Task<IEnumerable<Login>> SelecionarTodos()
        {
            return await _context.Login.ToListAsync();
        }
    }
}
