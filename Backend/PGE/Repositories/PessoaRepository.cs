using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PGE.Interfaces;
using PGE.Models;

namespace PGE.Repositories
{
    public class PessoaRepository : IPessoaRepository
    {
        private readonly PGEContext _context;

        public PessoaRepository(PGEContext context)
        {
            _context = context;
        }

        public void Alterar(Pessoa pessoa)
        {
            _context.Entry(pessoa).State = EntityState.Modified;
        }

        public void Excluir(Pessoa pessoa)
        {
            _context.Pessoa.Remove(pessoa);
        }

        public void Incluir(Pessoa pessoa)
        {
            _context.Pessoa.Add(pessoa);
        }

        public async Task<bool> Salvar()
        {
            return await _context.SaveChangesAsync() > 0; 
        }

        public async Task<Pessoa> SelecionarPelaPK(int id)
        {
            var pessoa = await _context.Pessoa.Where(x => x.Id == id).FirstOrDefaultAsync();
            return pessoa;
        }

        public async Task<IEnumerable<Pessoa>> SelecionarTodos()
        {
            return await _context.Pessoa.ToListAsync();
        }
    }
}
