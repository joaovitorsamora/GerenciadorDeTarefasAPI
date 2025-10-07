using GerenciadorDeTarefas.Models;

namespace GerenciadorDeTarefas.Repository.Interface
{
    public interface ITarefaRepository
    {
        Task<List<TarefaModel>> GetAllAsync();
        Task<List<TagModel>> GetAllTagAsync();
        Task<TarefaModel?> GetByIdAsync(int Id);
        Task PostAsync(TarefaModel tarefa);
        Task UpdateAsync(TarefaModel tarefa);
        Task DeleteAsync(TarefaModel tarefa);
        Task SaveChangesAsync();
    }
}
