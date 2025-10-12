namespace GerenciadorDeTarefas.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }

        public required string SenhaHash { get; set; }

        public ICollection<ProjetoModel> Projetos { get; set; } = new List<ProjetoModel>();
        public ICollection<TarefaModel> Tarefas { get; set; } = new List<TarefaModel>();
    }
}