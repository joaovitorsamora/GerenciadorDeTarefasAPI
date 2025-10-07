namespace GerenciadorDeTarefas.DTOs
{
    public class TagDTO
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public List<string>? Tarefas { get; set; }
    }
}
