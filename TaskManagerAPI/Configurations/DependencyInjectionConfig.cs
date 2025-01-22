using TaskManager.Application.Interfaces;
using TaskManager.Application.Services;
using TaskManager.Data.Repositories;
using TaskManager.Domain.Interfaces;

namespace TaskManagerAPI.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void AddDependencyInjectionConfig(this IServiceCollection services)
        {
            //***********repository***********
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IProjetoRepository, ProjetoRepository>();
            services.AddScoped<IHistoricoTarefaRepository, HistoricoTarefaRepository>();
            services.AddScoped<ITarefaRepository, TarefaRepository>();


            //***********services***********
            
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IProjetoService, ProjetoService>();
            services.AddScoped<ITarefaService, TarefaService>();
            // services.AddScoped<IHistoricoTarefaService, HistoricoTarefaService>();
        }
    }
}
