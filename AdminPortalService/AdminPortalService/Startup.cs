using AdminPortalService.AppConfig;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Reflection;
using TenantService;

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
            // https://stackoverflow.com/a/55541764/1175623
            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.Converters.Add(new StringEnumConverter()));
            services.AddApiVersioning();
            services.AddAutoMapper(typeof(Startup));

            #region Swagger

            services.AddSwaggerGen(options =>
            {
                options.IncludeXmlComments(@"SwaggerDocumentation.xml");
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

                options.OperationFilter<AddRequiredHeaderParameter>();
                options.OperationFilter<RemoveVersionFromParameter>();
                options.DocumentFilter<ReplaceVersionWithExactValueInPath>();
            });

            // https://stackoverflow.com/a/55541764/1175623
            services.AddSwaggerGenNewtonsoftSupport();
            #endregion Swagger

            // Registration for tenant
            services.RegisterTenantDependencies();
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
