using PGE.Models;
using System.Data.SqlTypes;

namespace PGE.Interfaces
{
    public interface ILoginRepository
    {
        void Incluir(Login login);
        void Alterar(Login login);
        void Excluir(Login login);
        Task<Login> SelecionarPelaPK(int id);
        Task<IEnumerable<Login>> SelecionarTodos();
        Task<bool> Salvar();
    }
}
