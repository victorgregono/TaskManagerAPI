using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Domain.Models;

namespace TaskManager.Domain.Interfaces
{
    public interface ITarefaRepository : IRepository<Tarefa>
    {
        Task<IEnumerable<Tarefa>> GetPendentesByProjetoIdAsync(int projetoId);
        Task<int> CountByProjetoIdAsync(int projetoId);
        Task<IEnumerable<Tarefa>> GetByProjetoIdAsync(int projetoId);
        Task CreateAsync(Tarefa tarefa);
        Task UpdateAsync(Tarefa tarefa);
        Task DeleteAsync(int id);

        Task<IEnumerable<DesempenhoUsuario>> GetPerformanceReport();
    }
}

