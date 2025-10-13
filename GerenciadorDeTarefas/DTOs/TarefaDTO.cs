namespace GerenciadorDeTarefas.DTOs
{
    public class TarefaDTO
    {
        public string? Titulo { get; set; }
        public DateTime DataCriacao { get; set; }
        public string? ProjetoNome { get; set; }
        public int UsuarioId { get; set; }
        public string? StatusTarefa { get; set; }
        public string? PrioridadeTarefa { get; set; }
        public List<string>? Tags { get; set; } = new();
    }
}
