using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace ReservasTucson.DataAccess.Support
{
    public static class Setup
    {
        /// <summary>
        ///  Base de Datos SQL Server
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomizedDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ReservasTucsonDBContext>(options =>

                options.UseSqlServer(configuration.GetConnectionString("ReservasTucsonBD"),
                
                b =>
                {
                    
                    b.CommandTimeout(180);
                })
            );

            return services;
        }

        /// <summary>
        /// Base de Datos en Memoria para Testing
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>

        public static IServiceCollection AddInMemoryDatabaseForTesting(this IServiceCollection services)
        {
            services.AddDbContext<ReservasTucsonDBContext>(options =>
                options.UseInMemoryDatabase("ReservasTucsonBD_Test")
            );

            return services;
        }        

    }    
}

