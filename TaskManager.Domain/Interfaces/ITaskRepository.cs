using TaskManager.Domain.Models;

namespace TaskManager.Domain.Interfaces
{
    public interface ITaskRepository : IRepository<Usuario>
    {
        Task<IEnumerable<Usuario>> GetAllRetentionAsync();
    }
}
