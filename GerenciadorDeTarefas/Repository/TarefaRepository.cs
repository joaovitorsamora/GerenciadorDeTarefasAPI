using GerenciadorDeTarefas.Data;
using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorDeTarefas.Repository
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly SistemaDeTarefaDBContext _context;

        public TarefaRepository(SistemaDeTarefaDBContext context)
        {
            _context = context;
        }

        public async Task<List<TarefaModel>> GetAllAsync()
        {
            return await _context.Tarefas
                .Include(t => t.Projeto)
                .Include(t => t.Usuario)
                .Include(t => t.Tags)
                .ToListAsync();
        }

        public async Task<List<TagModel>> GetAllTagAsync() => 
            await _context.Tags.ToListAsync();

        public async Task<TarefaModel?> GetByIdAsync(int id)
        {
            return await _context.Tarefas
                .Include(t => t.Projeto)
                .Include(t => t.Usuario)
                .Include(t => t.Tags)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task PostAsync(TarefaModel tarefa) 
        { 
           _context.Tarefas.Add(tarefa);

        }
        public async Task UpdateAsync(TarefaModel tarefa)
        {
           _context.Tarefas.Update(tarefa);
        }
        public async Task DeleteAsync(TarefaModel tarefa)
        {
            _context.Tarefas.Remove(tarefa);
        }

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
