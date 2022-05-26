using Microsoft.EntityFrameworkCore;
using MinimalAPIDemo.Data;
using MinimalAPIDemo.Settings;

namespace MinimalAPIDemo.Extensions
{
    public static class ContextDbExtensions
    {
        public static void AddContextDb(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = configuration.LoadSettings<AppSettings>("AppSettings");

            services.AddScoped(context =>
            {
                var options = new DbContextOptionsBuilder<MinimalContextDb>().UseSqlServer(appSettings.ConnectionString).Options;
                return new MinimalContextDb(options);
            });

            services.AddDbContext<MinimalContextDb>();
        }
    }
}
