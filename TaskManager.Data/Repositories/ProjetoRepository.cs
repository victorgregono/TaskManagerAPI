using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Data.SqlQuerys;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Models;

namespace TaskManager.Data.Repositories
{
    public class ProjetoRepository : Repository<Projeto>, IProjetoRepository
    {
        public ProjetoRepository(FactoryContext context) : base(context)
        {
        }

        //public async Task<IEnumerable<Projeto>> GetAllAsync()
        //{
        //    var sql = "SELECT * FROM Projeto";
        //    return await _connection.QueryAsync<Projeto>(sql);
        //}

        //public async Task<Projeto> GetByIdAsync(int id)
        //{
        //    var sql = "SELECT * FROM Projeto WHERE Id = @Id";
        //    return await _connection.QueryFirstOrDefaultAsync<Projeto>(sql, new { Id = id });
        //}

        public async Task CreateAsync(Projeto entity)
        {
            var sql = "INSERT INTO Projeto (Nome, UsuarioId) VALUES (@Nome, @UsuarioId)";
            await _context.Database
                .GetDbConnection()
                .ExecuteAsync(sql, entity);
        }

        public async Task UpdateAsync(Projeto entity)
        {
            var sql = "UPDATE Projeto SET Nome = @Nome, UsuarioId = @UsuarioId WHERE Id = @Id";
            await _context.Database
                .GetDbConnection()
                .ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(int id)
        {
            var sql = "DELETE FROM Projeto WHERE Id = @Id";
            await _context.Database
                .GetDbConnection()
                .ExecuteAsync(sql, new { Id = id });
        }

        public async Task<IEnumerable<Projeto>> GetAllByUsuarioIdAsync(int usuarioId)
        {
            var sql = "SELECT * FROM Projeto WHERE UsuarioId = @UsuarioId";
            return await _context.Database
                .GetDbConnection()
                .QueryAsync<Projeto>(sql, new { UsuarioId = usuarioId });
        }

        public async Task<IEnumerable<ProjetoDetalhado>> GetProjetosDetalhadosAsync()
        {

            return await _context.Database
                .GetDbConnection()
                .QueryAsync<ProjetoDetalhado>(ProjetoQueries.GetAllProject);
        }

        public async Task<Projeto> GetByNameAsync(string nome)
        {
            return await _context.Projetos
                .AsNoTracking()
                .AsSplitQuery()
                .Where(p => p.Nome == nome)
                .FirstOrDefaultAsync();
        }


    }
}
