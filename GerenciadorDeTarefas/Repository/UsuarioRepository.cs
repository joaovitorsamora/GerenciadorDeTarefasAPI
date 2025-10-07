using GerenciadorDeTarefas.Data;
using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Repository.Interface;
using Microsoft.EntityFrameworkCore;


namespace GerenciadorDeTarefas.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly SistemaDeTarefaDBContext _context;

        public UsuarioRepository(SistemaDeTarefaDBContext context)
        {
            _context = context;
        }

        public async Task<List<UsuarioModel>> GetAllAsync() =>
            await _context.Usuarios
                .Include(u => u.Projetos)
                    .ThenInclude(p => p.Tarefas)
                .Include(u => u.Tarefas)
                .ToListAsync();
        public async Task<UsuarioModel?> GetByIdAsync(int id) =>
            await _context.Usuarios.FindAsync(id);
        public async Task PostAsync(UsuarioModel usuario) 
        { 
           _context.Usuarios.Add(usuario);
        
        }
        public async Task UpdateAsync(UsuarioModel usuario)
        {
           _context.Usuarios.Update(usuario);
        } 
        
        public async Task DeleteAsync(UsuarioModel usuario)
        {
            _context.Usuarios.Remove(usuario);
        }

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
