using System.Collections.Generic;
using System.Threading.Tasks;
using ProjetoGerenciamento.Domain.Entities;

namespace ProjetoGerenciamento.Domain.Interfaces
{
    public interface IProjetoRepository
    {
        Task<bool> InserirAsync(Projeto projeto);
        Task<bool> AtualizarAsync(Projeto projeto);
        Task<bool> ExcluirAsync(int id);
        Task<Projeto> ObterPorIdAsync(int id);
        Task<IEnumerable<Projeto>> BuscarTodosAsync();
        Task<IEnumerable<Projeto>> BuscarPorNomeOuStatusAsync(string nome, string status);
    }
}