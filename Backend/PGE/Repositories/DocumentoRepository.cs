using Microsoft.EntityFrameworkCore;
using PGE.Interfaces;
using PGE.Models;

namespace PGE.Repositories
{
    public class DocumentoRepository : IDocumentoRepository
    {

        private readonly PGEContext _context;

        public DocumentoRepository(PGEContext context)
        {
            _context = context;
        }

        public void Alterar(Documento documento)
        {
            _context.Entry(documento).State = EntityState.Modified;
        }

        public void Excluir(Documento documento)
        {
            _context.Documento.Remove(documento);
        }

        public void Incluir(Documento documento)
        {
            _context.Documento.Add(documento);
        }

        public async Task<bool> Salvar()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Documento> SelecionarPelaPK(int id)
        {
            var documento = await _context.Documento.Where(x => x.Id == id).FirstOrDefaultAsync();
            return documento;
        }

        public async Task<IEnumerable<Documento>> SelecionarTodos()
        {
            return await _context.Documento.ToListAsync();
        }

    }
}
