namespace TaskManager.Domain.Models
{
    public class ProjetoDetalhado
    {
        public int ProjetoId { get; set; }
        public string ProjetoNome { get; set; }
        public string UsuarioNome { get; set; }
        public int? TarefaId { get; set; }
        public string TarefaTitulo { get; set; }
        public string TarefaDescricao { get; set; }
        public DateTime? TarefaDataVencimento { get; set; }
        public int? TarefaStatus { get; set; }
        public int? TarefaPrioridade { get; set; }
        public int? ComentarioId { get; set; }
        public string ComentarioTexto { get; set; }
        public DateTime? ComentarioData { get; set; }
        public int? HistoricoId { get; set; }
        public string HistoricoCampoModificado { get; set; }
        public string HistoricoValorAntigo { get; set; }
        public string HistoricoValorNovo { get; set; }
        public DateTime? HistoricoDataModificacao { get; set; }
    }
}
