using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Domain.Models;

[Table("Comentario")]
public class Comentario : BaseEntity
{
    [Key]
    public int Id { get; set; }    
    public required string Texto { get; set; }    
    public DateTime DataComentario { get; set; }   
    public int TarefaId { get; set; }

    [ForeignKey("TarefaId")]
    public Tarefa Tarefa { get; set; }   
    public int UsuarioId { get; set; }
    [ForeignKey("UsuarioId")]
    public Usuario Usuario { get; set; }
}
