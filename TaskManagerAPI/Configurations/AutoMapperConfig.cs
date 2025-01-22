using System.Reflection;
using TaskManager.Application.Profiles;

namespace TaskManagerAPI.Configurations
{
    public static class AutoMapperConfig
    {
        public static void AddAutoMapperConfig(this IServiceCollection services)
            => services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfiles)));
    }
}
