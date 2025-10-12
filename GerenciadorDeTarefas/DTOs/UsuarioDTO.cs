namespace GerenciadorDeTarefas.DTOs
{
    public class UsuarioResponseDTO
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
    }

    public class UsuarioRegisterDTO
    {
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
    }
}