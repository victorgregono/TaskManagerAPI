using Microsoft.EntityFrameworkCore;
using TaskManager.Data;

namespace TaskManagerAPI.Configurations
{
    public static class DatabaseConfig
    {
        public static void AddDatabaseConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FactoryContext>(options =>
                options.UseSqlite(GetConnectionString(configuration)));
        }

        private static string GetConnectionString(IConfiguration configuration)                    
            => configuration.GetConnectionString("DefaultConnection");
        
    }
}
