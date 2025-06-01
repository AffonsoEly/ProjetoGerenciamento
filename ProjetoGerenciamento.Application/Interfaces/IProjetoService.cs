using System.Collections.Generic;
using System.Threading.Tasks;
using ProjetoGerenciamento.Application.DTOs;

namespace ProjetoGerenciamento.Application.Interfaces
{
    public interface IProjetoService
    {
        Task<bool> InserirAsync(ProjetoDto projetoDto);
        Task<bool> AtualizarAsync(ProjetoDto projetoDto);
        Task<bool> ExcluirAsync(int id);
        Task<ProjetoDto> ObterPorIdAsync(int id);
        Task<IEnumerable<ProjetoDto>> BuscarTodosAsync();
        Task<IEnumerable<ProjetoDto>> BuscarPorNomeOuStatusAsync(string nome, string status);
    }
}
