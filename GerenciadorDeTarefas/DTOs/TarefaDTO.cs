namespace GerenciadorDeTarefas.DTOs
{
    public class TarefaDTO
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public DateTime DataCriacao { get; set; }
        public int ProjetoId { get; set; }
        public string? ProjetoNome { get; set; }
        public int UsuarioId { get; set; }
        public string? UsuarioNome { get; set; }
        public string? StatusTarefa { get; set; }
        public string? PrioridadeTarefa { get; set; }
        public List<string>? Tags { get; set; } = new();
    }
}
