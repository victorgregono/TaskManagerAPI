using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Extensions;
using TaskManager.Application.ViewModels;

namespace TaskManager.Application.Interfaces
{
    public interface ITarefaService
    {
        Task<OperationResult> FindAll();
        Task<OperationResult> FindByProjetoId(int id);
        Task<OperationResult> FindById(int id);
        Task<OperationResult> Create(TarefaViewModel tarefa);
        Task<OperationResult> Update(int id, TarefaViewModel tarefa);
        Task<OperationResult> Delete(int id);
        Task<OperationResult> GetPerformanceReport();
    }
}
