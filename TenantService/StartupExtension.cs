using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MongoDbClient;
using StackExchange.Redis;
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
            services.RegisterRedis();
        }

        public static void RegisterRedis(this IServiceCollection services)
        {
            // Redis
            services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect("192.168.99.101:6379"));
            services.AddSingleton<ICacheService, RedisCacheService>();
        }

        public static void RegisterServiceDependancies(this IServiceCollection services)
        {
            services.AddScoped<IUserMessageRepository, UserMessageRepository>();
        }

        // https://keestalkstech.com/2020/06/dependency-injection-based-on-request-headers/
        public static void RegisterDbDependancies(this IServiceCollection services)
        {
            //services.AddScoped<DatabaseContext>();

            services.AddScoped<IDatabaseContext>(provider =>
              {
                  var context = provider.GetRequiredService<IHttpContextAccessor>();
                  var cacheService = provider.GetRequiredService<ICacheService>();

                  var domainName = (bool)(context.HttpContext?.Request.Headers.ContainsKey("DomainName")) ? context.HttpContext?.Request.Headers["DomainName"].ToString() : null;

                  if (domainName == null) return null;

                  // Search in cache
                  var tenantObj = cacheService.GetCacheValueAsync(domainName).Result;
                  if (tenantObj == null) return null;

                  var tenant = Newtonsoft.Json.JsonConvert.DeserializeObject<Tenant>(tenantObj);
                  return new DatabaseContext(tenant.ConnectionString, tenant.DatabaseName);
              });
        }
    }
}
