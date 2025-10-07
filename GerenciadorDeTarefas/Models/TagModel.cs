namespace GerenciadorDeTarefas.Models
{
    public class TagModel
    {
        public int Id { get; set; }
        public string? Nome { get; set; } = string.Empty;
        public ICollection<TarefaModel>? Tarefas { get; set; }
    }
}
