using GerenciadorDeTarefas.Models;

namespace GerenciadorDeTarefas.Repository.Interface
{
    public interface ITagRepository
    {
        Task<List<TagModel>> GetAllAsync();
        Task<TagModel?> GetByIdAsync(int Id);
        Task PostAsync(TagModel tag);
        Task DeleteAsync(TagModel tag);

        Task<List<TarefaModel>> GetAllTarefaAsync(int Id);

        Task SaveChangesAsync();
    }
}
