using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Domain.Models;

[Table("Usuario")]
public class Usuario : BaseEntity
{
    public int Id { get; set; }
    public  string Nome { get; set; }
    public string Funcao { get; set; }
}


