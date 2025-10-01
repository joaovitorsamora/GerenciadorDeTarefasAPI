using GerenciadorDeTarefas.Models;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorDeTarefas.Data
{
    public class SistemaDeTarefaDBContext : DbContext
    {
        public SistemaDeTarefaDBContext(DbContextOptions<SistemaDeTarefaDBContext> options) : base(options)
        {

        }


        public DbSet<UsuarioModel> Usuarios { get; set; } 
        public DbSet<TarefaModel> Tarefas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }

}
