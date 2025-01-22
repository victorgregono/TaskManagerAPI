using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Models;

namespace TaskManager.Application.ViewModels
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }
        public  string Nome { get; set; }
        public  string Funcao { get; set; }
        //public ICollection<ProjetoViewModel> Projetos { get; set; }
    }
}
