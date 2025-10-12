using GerenciadorDeTarefas.Models;

namespace GerenciadorDeTarefas.Service.Interface
{
    public interface ITokenService
    {
        string GenerateToken(UsuarioModel user);
    }
}