using GerenciadorDeTarefas.Models;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorDeTarefas.Data
{
    public class SistemaDeTarefaDBContext : DbContext
    {
        public SistemaDeTarefaDBContext(DbContextOptions<SistemaDeTarefaDBContext> options) : base(options){}
        public DbSet<UsuarioModel> Usuarios { get; set; } 
        public DbSet<ProjetoModel> Projetos { get; set; }
        public DbSet<TarefaModel> Tarefas { get; set; }
        public DbSet<TagModel> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TarefaModel>()
                .HasMany(t => t.Tags)
                .WithMany(t => t.Tarefas);
        }

    }

}
