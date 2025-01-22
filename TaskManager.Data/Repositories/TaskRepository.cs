using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Models;
using Dapper;
using TaskManager.Data.SqlQuerys;

namespace TaskManager.Data.Repositories
{
    public class TaskRepository : Repository<Usuario>, ITaskRepository
    {
        public TaskRepository(FactoryContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Usuario>> GetAllRetentionAsync()
        {
            var connection = _context.Database.GetDbConnection();

            // Ordem correta para exclusão das tabelas
            await connection.ExecuteAsync(SqlScripts.DropHistoricoTarefaTable);
            await connection.ExecuteAsync(SqlScripts.DropComentarioTable);
            await connection.ExecuteAsync(SqlScripts.DropTarefaTable);
            await connection.ExecuteAsync(SqlScripts.DropProjetoTable);
            await connection.ExecuteAsync(SqlScripts.DropUsuarioTable);

            // Executa os scripts de criação das tabelas na ordem correta
            await connection.ExecuteAsync(SqlScripts.CreateUsuarioTable);
            await connection.ExecuteAsync(SqlScripts.CreateProjetoTable);
            await connection.ExecuteAsync(SqlScripts.CreateTarefaTable);
            await connection.ExecuteAsync(SqlScripts.CreateComentarioTable);
            await connection.ExecuteAsync(SqlScripts.CreateHistoricoTarefaTable);

            // Inserir dados fictícios
            var sql = SqlScripts.InsertFakeData;
            await connection.ExecuteAsync(sql);

            var saida = await connection.QueryAsync<Usuario>("SELECT * FROM Usuario");

            var teste = await _currentSet
                       .AsNoTracking()
                       .AsSplitQuery()
                       .ToListAsync();

            return await connection.QueryAsync<Usuario>("SELECT * FROM Usuario");
        }
    }
}
