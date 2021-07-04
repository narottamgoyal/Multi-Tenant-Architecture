using AdminPortalService.AppConfig;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDbClient;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Reflection;
using TenantService;
using UserManagement.Persistence;

namespace AdminPortalService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddApiVersioning();
            services.AddAutoMapper(typeof(Startup));

            #region Swagger

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Admin Portal Service", Version = "v1" });

                options.DocInclusionPredicate((version, desc) =>
                {
                    if (!desc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;
                    var versions = methodInfo.DeclaringType
                        .GetCustomAttributes(false)
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);
                    return versions.Any(v => $"v{v}" == version);
                });

                options.OperationFilter<RemoveVersionFromParameter>();
                options.DocumentFilter<ReplaceVersionWithExactValueInPath>();
            });

            #endregion Swagger

            // Registration for tenant
            RegisterTenantDependencies(services);
        }

        private static void RegisterTenantDependencies(IServiceCollection services)
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

            // Redis
            services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect("192.168.99.101:6379"));
            services.AddSingleton<ICacheService, RedisCacheService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                #region Swagger
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"Admin Portal Service v1");
                });
                #endregion Swagger
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
