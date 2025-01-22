using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Models;

namespace TaskManager.Application.ViewModels
{
    public class TarefaViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataVencimento { get; set; }
        public StatusTarefa Status { get; set; }
        public PrioridadeTarefa Prioridade { get; set; }
        public int ProjetoId { get; set; }
        //public ProjetoViewModel Projeto { get; set; }
        //public ICollection<ComentarioViewModel> Comentarios { get; set; }
        //public ICollection<HistoricoTarefaViewModel> Historicos { get; set; }
    }
}
