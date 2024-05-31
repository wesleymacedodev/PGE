using PGE.Models;

namespace PGE.Interfaces
{
    public interface IProcessoRepository
    {
        void Incluir(Processo processo);
        void Alterar(Processo processo);
        void Excluir(Processo processo);
        Task<Processo> SelecionarPelaPK(int id);
        Task<IEnumerable<Processo>> SelecionarTodos();
        Task<bool> Salvar();
    }
}
