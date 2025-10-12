using GerenciadorDeTarefas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Npgsql.EntityFrameworkCore.PostgreSQL; 

namespace GerenciadorDeTarefas.Data
{
    public class SistemaDeTarefaDBContext : DbContext
    {
        public SistemaDeTarefaDBContext(DbContextOptions<SistemaDeTarefaDBContext> options) : base(options) { }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           string connectionString = "DefaultConnection";

            optionsBuilder
                .UseNpgsql(connectionString, o =>
                {
                    o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                })
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        }
    }
}