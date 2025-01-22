using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Domain.Models;

[Table("Projeto")]
public class Projeto : BaseEntity
{
    [Key]
    public int Id { get; set; }    
    public required string Nome { get; set; }    
    public int UsuarioId { get; set; }

    [ForeignKey("UsuarioId")]
    public Usuario Usuario { get; set; }

    // Relacionamento com Tarefa
    public ICollection<Tarefa> Tarefas { get; set; }
}


