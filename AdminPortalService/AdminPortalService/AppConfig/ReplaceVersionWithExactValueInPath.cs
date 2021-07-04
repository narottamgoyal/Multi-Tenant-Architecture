using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AdminPortalService.AppConfig
{
    public class ReplaceVersionWithExactValueInPath : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var op = new OpenApiPaths();
            foreach (var (emptyKey, value) in swaggerDoc.Paths)
            {
                var completeKey = emptyKey.Replace("v{version}", swaggerDoc.Info.Version);
                op.Add(completeKey, value);
            }
            swaggerDoc.Paths = op;
        }
    }
}
