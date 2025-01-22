using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Domain.Models;

[Table("Tarefa")]
public class Tarefa : BaseEntity
{
    [Key]
    public int Id { get; set; }    
    public  string Titulo { get; set; }

    public  string Descricao { get; set; }
   
    public DateTime DataVencimento { get; set; }
    
    public int Status { get; set; }
    
    public int Prioridade { get; set; }
    
    public int ProjetoId { get; set; }

    [ForeignKey("ProjetoId")]
    public Projeto Projeto { get; set; }

    // Relacionamento com Comentario e HistoricoTarefa
    public ICollection<Comentario> Comentarios { get; set; }
    public ICollection<HistoricoTarefa> Historicos { get; set; }
}