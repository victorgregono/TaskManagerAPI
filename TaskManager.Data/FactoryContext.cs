using Microsoft.EntityFrameworkCore;
using TaskManager.Data.SqlQuerys;
using TaskManager.Domain.Models;

namespace TaskManager.Data
{
    public class FactoryContext : DbContext    
    {
        public FactoryContext(DbContextOptions<FactoryContext> options): base(options)
        {            
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<HistoricoTarefa> HistoricoTarefas { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    // Configurações adicionais de mapeamento podem ser feitas aqui


        public async Task InitializeDatabaseAsync()
        {
            var connection = Database.GetDbConnection();
            await connection.OpenAsync();

            using (var command = connection.CreateCommand())
            {
                // Ordem correta para exclusão das tabelas
                command.CommandText = SqlScripts.DropHistoricoTarefaTable;
                await command.ExecuteNonQueryAsync();

                command.CommandText = SqlScripts.DropComentarioTable;
                await command.ExecuteNonQueryAsync();

                command.CommandText = SqlScripts.DropTarefaTable;
                await command.ExecuteNonQueryAsync();

                command.CommandText = SqlScripts.DropProjetoTable;
                await command.ExecuteNonQueryAsync();

                command.CommandText = SqlScripts.DropUsuarioTable;
                await command.ExecuteNonQueryAsync();

                // Executa os scripts de criação das tabelas na ordem correta
                command.CommandText = SqlScripts.CreateUsuarioTable;
                await command.ExecuteNonQueryAsync();

                command.CommandText = SqlScripts.CreateProjetoTable;
                await command.ExecuteNonQueryAsync();

                command.CommandText = SqlScripts.CreateTarefaTable;
                await command.ExecuteNonQueryAsync();

                command.CommandText = SqlScripts.CreateComentarioTable;
                await command.ExecuteNonQueryAsync();

                command.CommandText = SqlScripts.CreateHistoricoTarefaTable;
                await command.ExecuteNonQueryAsync();

                // Inserir dados fictícios
                command.CommandText = SqlScripts.InsertFakeData;
                await command.ExecuteNonQueryAsync();
            }

            await connection.CloseAsync();
        }



    }
}
