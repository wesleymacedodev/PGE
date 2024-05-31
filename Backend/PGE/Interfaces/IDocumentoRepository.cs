using PGE.Models;

namespace PGE.Interfaces
{
    public interface IDocumentoRepository
    {
        void Incluir(Documento documento);
        void Alterar(Documento documento);
        void Excluir(Documento documento);
        Task<Documento> SelecionarPelaPK(int id);
        Task<IEnumerable<Documento>> SelecionarTodos();
        Task<bool> Salvar();
    }
}
