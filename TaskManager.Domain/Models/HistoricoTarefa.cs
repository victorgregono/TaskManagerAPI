
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Domain.Models;

[Table("HistoricoTarefa")]
public class HistoricoTarefa : BaseEntity
{
    [Key]
    public int Id { get; set; }

   
    public required string CampoModificado { get; set; }

    public string ValorAntigo { get; set; }
    public string ValorNovo { get; set; }

    
    public DateTime DataModificacao { get; set; }

    
    public int TarefaId { get; set; }

    [ForeignKey("TarefaId")]
    public Tarefa Tarefa { get; set; }

    public int UsuarioId { get; set; }

    [ForeignKey("UsuarioId")]
    public Usuario Usuario { get; set; }
}

