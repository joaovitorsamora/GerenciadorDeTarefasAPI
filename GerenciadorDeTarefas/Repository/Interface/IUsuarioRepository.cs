using GerenciadorDeTarefas.Models;

namespace GerenciadorDeTarefas.Repository.Interface
{
    public interface IUsuarioRepository
    {
        Task<List<UsuarioModel>> GetAllAsync();
        Task<UsuarioModel> GetByIdAsync(int Id);
        Task<UsuarioModel> AddAsync(UsuarioModel usuario);
        Task<UsuarioModel> UpdateAsync(int Id, UsuarioModel usuario);
        Task<UsuarioModel> DeleteAsync(int Id);
        Task SaveChangesAsync();
    }


    
}
