using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mongo.Common.Settings;

namespace Mongo.Common.MongoDB
{
    public static class ServiceExtensions
    {
        public static void ConfigureMongoConnection(this IServiceCollection services, IConfiguration configuration) =>
            services.Configure<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)));
    }
}
