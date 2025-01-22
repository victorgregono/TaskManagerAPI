using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Extensions;
using TaskManager.Application.ViewModels;

namespace TaskManager.Application.Interfaces
{
    public interface IHistoricoTarefaService
    {
        Task<OperationResult> FindAll();
        Task<OperationResult> FindById(int id);
        Task<OperationResult> Create(HistoricoTarefaViewModel historico);
        Task<OperationResult> Update(int id, HistoricoTarefaViewModel historico);
        Task<OperationResult> Delete(int id);
    }
}
