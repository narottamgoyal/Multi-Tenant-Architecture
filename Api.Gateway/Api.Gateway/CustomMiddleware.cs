using Microsoft.AspNetCore.Http;
using Ocelot.Middleware;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Gateway
{
    public class CustomMiddleware : OcelotPipelineConfiguration
    {
        public CustomMiddleware()
        {
            PreAuthorizationMiddleware = async (ctx, next) =>
            {
                await ProcessRequest(ctx, next);
            };
        }

        public async Task ProcessRequest(HttpContext context, System.Func<Task> next)
        {
            try
            {
                var user = ((DefaultHttpContext)context)?.User;
                EnrichClaim(user);
                await next.Invoke();
            }
            catch (System.Exception)
            {
                await ReturnStatus(context, HttpStatusCode.InternalServerError, "some error");
            }
        }

        private void EnrichClaim(ClaimsPrincipal claims)
        {
            var listOfClaims = new List<Claim>
            {
                new Claim("CustomClaimName", "CustomClaimValue")
            };

            claims.AddIdentity(new ClaimsIdentity(listOfClaims));
        }

        private static async Task ReturnStatus(HttpContext context, HttpStatusCode statusCode, string msg)
        {
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(msg);
        }
    }
}
