using Microsoft.EntityFrameworkCore;
using PGE.Interfaces;
using PGE.Models;

namespace PGE.Repositories
{
    public class DistribuirRepository : IDistribuirRepository
    {
        private readonly PGEContext _context;

        public DistribuirRepository(PGEContext context)
        {
            _context = context;
        }

        public void Alterar(Distribuir distribuir)
        {
            _context.Entry(distribuir).State = EntityState.Modified;
        }

        public void Excluir(Distribuir distribuir)
        {
            _context.Distribuir.Remove(distribuir);
        }

        public void Incluir(Distribuir distribuir)
        {
            _context.Distribuir.Add(distribuir);
        }

        public async Task<bool> Salvar()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Distribuir> SelecionarPelaPK(int id)
        {
            var distribuir = await _context.Distribuir.Where(x => x.Id == id).FirstOrDefaultAsync();
            return distribuir;
        }

        public async Task<IEnumerable<Distribuir>> SelecionarTodos()
        {
            return await _context.Distribuir.ToListAsync();
        }
    }
}
