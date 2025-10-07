using GerenciadorDeTarefas.Data;
using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorDeTarefas.Repository
{
    public class TagRepository : ITagRepository
    {
        private readonly SistemaDeTarefaDBContext _context;
        public TagRepository(SistemaDeTarefaDBContext context)
        {
            _context = context;
        }


        public async Task<List<TagModel>> GetAllAsync() =>
            await _context.Tags.ToListAsync();

        public async Task<TagModel?> GetByIdAsync(int id) =>
            await _context.Tags.FindAsync(id);
        public async Task PostAsync(TagModel tag)
        { 
            _context.Tags.Add(tag);
        }

        public async Task<List<TarefaModel>> GetAllTarefaAsync(int tagId)
        {
            return await _context.Tarefas
                .Where(t => t.Tags.Any(tag => tag.Id == tagId))
                .ToListAsync();
        }

        public async Task DeleteAsync(TagModel tag)
        {
            _context.Tags.Remove(tag);
        }

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
