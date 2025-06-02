using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProjetoGerenciamento.Infrastructure.Data;

namespace ProjetoGerenciamento.Infrastructure.Data
{
    public class ProjetoDbContextFactory : IDesignTimeDbContextFactory<ProjetoDbContext>
    {
        public ProjetoDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProjetoDbContext>();

            // 🔐 Substitua com sua string de conexão real
            var connectionString = "Host=localhost;Port=5432;Database=ProjetoGerenciamentoDb;Username=postgres;Password=123456";

            optionsBuilder.UseNpgsql(connectionString);

            return new ProjetoDbContext(optionsBuilder.Options);
        }
    }
}
