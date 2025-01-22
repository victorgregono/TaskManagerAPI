using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Extensions;
using TaskManager.Application.ViewModels;

namespace TaskManager.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<OperationResult> FindAll();
        Task<OperationResult> FindById(int id);
        Task<OperationResult> Create(UsuarioViewModel usuario);        
        
    }
}
