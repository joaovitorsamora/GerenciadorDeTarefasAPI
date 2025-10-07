namespace GerenciadorDeTarefas.DTOs
{
    public class ProjetoDTO
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public int UsuarioId { get; set; }
        public string? UsuarioNome { get; set; }
        public int ProjetoId { get; set; }
        public List<TarefaDTO>? Tarefas { get; set; }
    }
}
