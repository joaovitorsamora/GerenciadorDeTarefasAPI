using GerenciadorDeTarefas.Models;

namespace GerenciadorDeTarefas.Repository.Interface
{
    public interface IUsuarioRepository
    {
        Task<List<UsuarioModel>> GetAllAsync();
        Task<UsuarioModel?> GetByIdAsync(int Id);
        Task PostAsync(UsuarioModel usuario);
        Task UpdateAsync(UsuarioModel usuario);
        Task DeleteAsync(UsuarioModel usuario);
        Task SaveChangesAsync();
    }


    
}
