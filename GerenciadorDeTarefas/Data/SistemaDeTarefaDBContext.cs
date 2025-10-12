﻿using GerenciadorDeTarefas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Npgsql.EntityFrameworkCore.PostgreSQL; 

namespace GerenciadorDeTarefas.Data
{
    public class SistemaDeTarefaDBContext : DbContext
    {

        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<ProjetoModel> Projetos { get; set; }
        public DbSet<TarefaModel> Tarefas { get; set; }
        public DbSet<TagModel> Tags { get; set; }

        public SistemaDeTarefaDBContext(DbContextOptions<SistemaDeTarefaDBContext> options) : base(options) { }
        

    }
}