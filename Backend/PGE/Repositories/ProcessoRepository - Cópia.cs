using Microsoft.EntityFrameworkCore;
using PGE.Interfaces;
using PGE.Models;

namespace PGE.Repositories
{
    public class ProcessoRepository : IProcessoRepository
    {
        // atribuindo a referencia no contexto do repository 
        private readonly PGEContext _context;

        public ProcessoRepository(PGEContext context)
        {
            _context = context;
        }
        // contexto

        public void Alterar(Processo processo)
        {
            _context.Entry(processo).State = EntityState.Modified;
        }

        public void Excluir(Processo processo)
        {
            _context.Processo.Remove(processo);
        }

        public void Incluir(Processo processo)
        {
            _context.Processo.Add(processo);
        }

        public async Task<bool> Salvar()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Processo> SelecionarPelaPK(int id)
        {
            var processo = await _context.Processo.Where(x => x.Id == id).FirstOrDefaultAsync();
            return processo;
        }

        public async Task<IEnumerable<Processo>> SelecionarTodos()
        {
            return await _context.Processo.ToListAsync();
        }
    }
}
