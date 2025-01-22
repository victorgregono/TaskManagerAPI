using Dapper;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Models;

namespace TaskManager.Data.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(FactoryContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Usuario>> GetByFuncaoAsync(string funcao)
                => await _context.Usuarios
                    .AsNoTracking()
                    .AsSplitQuery()
                    .Where(u => EF.Functions.Collate(u.Funcao, "NOCASE") == funcao)
                    .ToListAsync();


        //*************************************************
        //usando dapper, caso queira usar dapper, descomente
        //o código abaixo e comente o código acima

        //public async Task<IEnumerable<Usuario>> GetByFuncaoAsync(string funcao)
        //=> await _context.Database
        //    .GetDbConnection()
        //    .QueryAsync<Usuario>("SELECT * FROM Usuario WHERE Funcao COLLATE NOCASE = @funcao", new { funcao });


    }
}
