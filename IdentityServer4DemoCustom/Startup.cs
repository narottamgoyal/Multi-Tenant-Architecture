using IdentityServer4;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDbClient;
using Serilog;
using StackExchange.Redis;
using TenantService;
using UserManagement.Persistence;

namespace IdentityServer4Demo
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // cookie policy to deal with temporary browser incompatibilities
            services.AddSameSiteCookiePolicy();

            services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryClients(Config.GetClients())
                .AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>()
                .AddDeveloperSigningCredential(persistKey: false);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie()
            #region commented
               
            #endregion
                .AddLocalApi(options =>
                {
                    options.ExpectedScope = "api";
                });

            // preserve OIDC state in cache (solves problems with AAD and URL lenghts)
            services.AddOidcStateDataFormatterCache("aad");

            // add CORS policy for non-IdentityServer endpoints
            services.AddCors(options =>
            {
                options.AddPolicy("api", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            // demo versions (never use in production)
            // services.AddTransient<IProfileService, CustomProfileService>();
            services.AddTransient<IRedirectUriValidator, DemoRedirectValidator>();
            services.AddTransient<ICorsPolicyService, DemoCorsPolicy>();

            // Registration for tenant
            RegisterTenantDependencies(services);

            // Custom Password Validator
            services.AddTransient<IResourceOwnerPasswordValidator, CustomResourceOwnerPasswordValidator>();
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

        public void Configure(IApplicationBuilder app)
        {
            app.UseCookiePolicy();
            app.UseSerilogRequestLogging();
            app.UseDeveloperExceptionPage();

            app.UseCors("api");

            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}