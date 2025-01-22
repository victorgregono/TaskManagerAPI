using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain.Models
{
    public class DesempenhoUsuario()
    {
       public string NomeUsuario { get; set; }
        public int TarefasConcluidas { get; set; }
        public double MediaTarefasPorDia { get; set; }
    }
    
}
