namespace GerenciadorDeTarefas.Models
{
    public class ProjetoModel
    {
        public int Id { get; set; }
        public string? Nome { get; set; }


        public int UsuarioId { get; set; }
        public string? UsuarioNome { get; set; } = null!;

        public int ProjetoId { get; set; }

        public ICollection<TarefaModel> Tarefas { get; set; } = new List<TarefaModel>();

        public ICollection<TagModel> Tags { get; set; } = new List<TagModel>();

    }
}
