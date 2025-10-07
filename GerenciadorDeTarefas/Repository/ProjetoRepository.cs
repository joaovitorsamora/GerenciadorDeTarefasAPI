using GerenciadorDeTarefas.Data;
using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorDeTarefas.Repository
{
    public class ProjetoRepository : IProjetoRepository
    {
        private readonly SistemaDeTarefaDBContext _context;
        public ProjetoRepository(SistemaDeTarefaDBContext context)
        {
            _context = context;
        }

        public async Task<ProjetoModel?> GetByIdAsync(int id)
        {
            return await _context.Projetos
                .Include(p => p.Tarefas)
                    .ThenInclude(t => t.Usuario)
                .Include(p => p.Tarefas)
                    .ThenInclude(t => t.Tags)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<ProjetoModel>> GetAllAsync()
        {
            return await _context.Projetos
                .Include(p => p.Tarefas)
                    .ThenInclude(t => t.Tags)
                .Include(p => p.Tarefas)
                    .ThenInclude(t => t.Usuario)
                .ToListAsync();
        }

        public async Task<List<TarefaModel>> GetAllTarefaAsync()
        {
            return await _context.Tarefas
                .Include(t => t.Projeto)
                .Include(t => t.Usuario)
                .Include(t => t.Tags)
                .ToListAsync();
        }

        public async Task<List<TagModel>> GetAllTagAsync() =>
            await _context.Tags.ToListAsync();
        public async Task PostAsync(ProjetoModel projeto)
        {
            _context.Projetos.Add(projeto);

        }
        public async Task SaveChangesAsync() =>
           await _context.SaveChangesAsync();
    }
}
