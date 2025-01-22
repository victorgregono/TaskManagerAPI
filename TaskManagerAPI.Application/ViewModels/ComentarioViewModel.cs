using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Models;

namespace TaskManager.Application.ViewModels
{
    public class ComentarioViewModel
    {
        public int Id { get; set; }
        public string Texto { get; set; }
        public DateTime DataComentario { get; set; }
        public int TarefaId { get; set; }
        public TarefaViewModel Tarefa { get; set; }
        public int UsuarioId { get; set; }
        public UsuarioViewModel Usuario { get; set; }
    }
}
