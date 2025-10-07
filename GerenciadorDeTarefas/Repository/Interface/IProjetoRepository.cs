using GerenciadorDeTarefas.Models;

namespace GerenciadorDeTarefas.Repository.Interface
{
    public interface IProjetoRepository
    {
        Task<ProjetoModel?> GetByIdAsync(int Id);
        Task<List<ProjetoModel>> GetAllAsync();
        Task<List<TarefaModel>> GetAllTarefaAsync();
        Task<List<TagModel>> GetAllTagAsync();
        Task PostAsync(ProjetoModel projeto);
        Task SaveChangesAsync();

    }
}
