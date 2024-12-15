using MerosWebApi.Core.Repository;
using MerosWebApi.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MerosWebApi.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDB"));

            services.AddSingleton<MongoDbService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMeroRepository, MeroRepository>();

            return services;
        }
    }
}
