using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Domain.Models;

namespace TaskManager.Domain.Interfaces
{
    public interface IProjetoRepository : IRepository<Projeto>
    {
        Task<IEnumerable<ProjetoDetalhado>> GetProjetosDetalhadosAsync();
        Task CreateAsync(Projeto projeto);
        Task UpdateAsync(Projeto projeto);
        Task DeleteAsync(int id);
        Task<Projeto> GetByNameAsync(string nome);

    }
}
