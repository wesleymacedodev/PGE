using PGE.Models;

namespace PGE.Interfaces
{
    public interface IDistribuirRepository
    {
        void Incluir(Distribuir distribuir);
        void Alterar(Distribuir distribuir);
        void Excluir(Distribuir distribuir);
        Task<Distribuir> SelecionarPelaPK(int id);
        Task<IEnumerable<Distribuir>> SelecionarTodos();
        Task<bool> Salvar();
    }
}
