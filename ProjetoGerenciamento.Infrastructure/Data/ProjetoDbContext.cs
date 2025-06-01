using Microsoft.EntityFrameworkCore;
using ProjetoGerenciamento.Domain.Entities;
using ProjetoGerenciamento.Domain.Enums;
namespace ProjetoGerenciamento.Infrastructure.Data
{
    public class ProjetoDbContext : DbContext
    {
        public ProjetoDbContext(DbContextOptions<ProjetoDbContext> options) : base(options) { }

        public DbSet<Projeto> Projetos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configurações específicas, se precisar
        }
    }
}
