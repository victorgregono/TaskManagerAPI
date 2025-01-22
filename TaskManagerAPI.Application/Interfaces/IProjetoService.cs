using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Extensions;
using TaskManager.Application.ViewModels;

namespace TaskManager.Application.Interfaces
{
    public interface IProjetoService
    {
        Task<OperationResult> FindAll();
        Task<OperationResult> FindById(int id);
        Task<OperationResult> Create(ProjetoViewModel projeto);
        Task<OperationResult> Update(int id, ProjetoViewModel projeto);
        Task<OperationResult> Delete(int id);
    }
}
