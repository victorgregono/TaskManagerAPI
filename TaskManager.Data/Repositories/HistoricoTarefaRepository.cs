using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Models;

namespace TaskManager.Data.Repositories
{
    public class HistoricoTarefaRepository : Repository<HistoricoTarefa>, IHistoricoTarefaRepository
    {
        public HistoricoTarefaRepository(FactoryContext context) : base(context)
        {
        }

        public async Task<IEnumerable<HistoricoTarefa>> GetByTarefaIdAsync(int tarefaId)
        {
            var sql = "SELECT * FROM HistoricoTarefa WHERE TarefaId = @TarefaId";
            return await _context.Database
                .GetDbConnection()
                .QueryAsync<HistoricoTarefa>(sql, new { TarefaId = tarefaId });
        }

        public async Task CreateAsync(HistoricoTarefa entity)
        {
            var sql = "INSERT INTO HistoricoTarefa (CampoModificado, ValorAntigo, ValorNovo, DataModificacao, TarefaId, UsuarioId) VALUES (@CampoModificado, @ValorAntigo, @ValorNovo, @DataModificacao, @TarefaId, @UsuarioId)";
            await _context.Database
                .GetDbConnection()
                .ExecuteAsync(sql, entity);
        }

        public async Task UpdateAsync(HistoricoTarefa entity)
        {
            var sql = "UPDATE HistoricoTarefa SET CampoModificado = @CampoModificado, ValorAntigo = @ValorAntigo, ValorNovo = @ValorNovo, DataModificacao = @DataModificacao, TarefaId = @TarefaId, UsuarioId = @UsuarioId WHERE Id = @Id";
            await _context.Database
                .GetDbConnection()
                .ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(int id)
        {
            var sql = "DELETE FROM HistoricoTarefa WHERE Id = @Id";
            await _context.Database
                .GetDbConnection()
                .ExecuteAsync(sql, new { Id = id });
        }
    }
}
