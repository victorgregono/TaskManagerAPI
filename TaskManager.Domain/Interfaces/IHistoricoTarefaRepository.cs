using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Domain.Models;

namespace TaskManager.Domain.Interfaces
{
    public interface IHistoricoTarefaRepository : IRepository<HistoricoTarefa>
    {
        Task<IEnumerable<HistoricoTarefa>> GetByTarefaIdAsync(int tarefaId);
        Task CreateAsync(HistoricoTarefa historicoTarefa);
    }
}

