using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.SqlQueries;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Models;

namespace TaskManager.Data.Repositories
{
    public class TarefaRepository : Repository<Tarefa>, ITarefaRepository
    {
        public TarefaRepository(FactoryContext context) : base(context)
        {
        }

        public async Task<int> CountByProjetoIdAsync(int projetoId)
        {
            return await _context.Tarefas
                .Where(t => t.ProjetoId == projetoId)
                .CountAsync();
        }

        public Task CreateAsync(Tarefa tarefa)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tarefa>> GetByProjetoIdAsync(int projetoId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Tarefa>> GetPendentesByProjetoIdAsync(int projetoId)
        {
            return await _context.Set<Tarefa>()
                .Where(t => t.ProjetoId == projetoId && t.Status == (int)StatusTarefa.Pendente)
                .ToListAsync();
        }

        public async Task<IEnumerable<DesempenhoUsuario>> GetPerformanceReport()
        {
            var desempenho = await _context.Database
                .GetDbConnection()
                .QueryAsync<DesempenhoUsuario>(TarefaQueries.GetPerformanceReport);
            return desempenho;
        }

    }
}
