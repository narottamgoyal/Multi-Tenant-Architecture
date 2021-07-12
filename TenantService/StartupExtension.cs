using Microsoft.Extensions.DependencyInjection;
using MongoDbClient;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Persistence;

namespace TenantService
{
    public static class StartupExtension
    {
        public static void RegisterTenantDependencies(this IServiceCollection services)
        {
            // Master Database
            string ConnectionString = "mongodb://localhost:27017";
            string DatabaseName = "InfinitePOC_MasterDb";
            services.AddSingleton<IDatabaseContext>(new DatabaseContext(ConnectionString, DatabaseName));
            services.AddSingleton<IMasterDbUserRepository, MasterDbUserRepository>();
            services.AddSingleton<IMasterDbTenantRepository, MasterDbTenantRepository>();
            services.AddSingleton<IMongoDbQueryRepository, MongoDbQueryRepository>();
            services.AddSingleton<ITenantDbUserRepository, TenantDbUserRepository>();

            // Service
            services.AddSingleton<IUserDetailService, UserDetailService>();
            services.AddSingleton<ITenantDetailService, TenantDetailService>();

            // Redis
            services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect("192.168.99.101:6379"));
            services.AddSingleton<ICacheService, RedisCacheService>();
        }
    }
}
