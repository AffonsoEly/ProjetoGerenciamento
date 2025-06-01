using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjetoGerenciamento.Domain.Entities;
using ProjetoGerenciamento.Domain.Interfaces;
using ProjetoGerenciamento.Domain.Enums;
using ProjetoGerenciamento.Infrastructure.Data;
namespace ProjetoGerenciamento.Infrastructure.Repositories
{
    public class ProjetoRepository : IProjetoRepository
    {
        private readonly ProjetoDbContext _context;

        public ProjetoRepository(ProjetoDbContext context)
        {
            _context = context;
        }

        public async Task<bool> InserirAsync(Projeto projeto)
        {
            await _context.Projetos.AddAsync(projeto);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AtualizarAsync(Projeto projeto)
        {
            _context.Projetos.Update(projeto);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            var projeto = await _context.Projetos.FindAsync(id);
            if (projeto == null) return false;
            _context.Projetos.Remove(projeto);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Projeto> ObterPorIdAsync(int id)
        {
            return await _context.Projetos.FindAsync(id);
        }

        public async Task<IEnumerable<Projeto>> BuscarTodosAsync()
        {
            return await _context.Projetos.ToListAsync();
        }

        public async Task<IEnumerable<Projeto>> BuscarPorNomeOuStatusAsync(string nome, string status)
        {
            var query = _context.Projetos.AsQueryable();

            if (!string.IsNullOrEmpty(nome))
                query = query.Where(p => p.Nome.Contains(nome));

            if (!string.IsNullOrEmpty(status))
            {
                if (Enum.TryParse<Domain.Enums.StatusProjeto>(status, out var statusEnum))
                    query = query.Where(p => p.Status == statusEnum);
            }

            return await query.ToListAsync();
        }
    }
}
