using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using MerosWebApi.Core.Repository;
using MerosWebApi.Persistence.Repositories;

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
