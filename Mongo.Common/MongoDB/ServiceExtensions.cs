using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mongo.Common.Settings;

namespace Mongo.Common.MongoDB
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureMongoSettings(this IServiceCollection services, string connectionString, string dataBaseName)
        {
            services.AddSingleton(_ =>
            {
                return MongoDbSettings.Initialize(connectionString, dataBaseName);
            });
            return services;
        }
    }
}