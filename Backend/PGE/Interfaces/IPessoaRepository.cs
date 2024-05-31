using PGE.Models;
using System.Data.SqlTypes;

namespace PGE.Interfaces
{
    public interface IPessoaRepository
    {
        void Incluir(Pessoa pessoa);
        void Alterar(Pessoa pessoa);
        void Excluir(Pessoa pessoa);
        Task<Pessoa> SelecionarPelaPK(int id);
        Task<IEnumerable<Pessoa>> SelecionarTodos();
        Task<bool> Salvar();
    }
}
