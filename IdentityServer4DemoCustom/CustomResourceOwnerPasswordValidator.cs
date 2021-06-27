using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TenantService;
using UserManagement.Persistence;

namespace IdentityServer4Demo
{
    public class CustomResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserDetailService _userService;

        public CustomResourceOwnerPasswordValidator(IUserDetailService userService)
        {
            _userService = userService;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                // UserName means email id in this case
                var user = await _userService.GetUserAsync(context.UserName);
                if (user != null)
                {
                    // Validation
                    if (user.Password == context.Password)
                    {
                        //set the result
                        context.Result = new GrantValidationResult(
                            subject: user.Id.ToString(),
                            authenticationMethod: "custom",
                            claims: GetUserClaims(user));

                        return;
                    }

                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Incorrect password");
                    return;
                }
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "User does not exist.");
                return;
            }
            catch (Exception)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid username or password");
            }
        }

        public static List<Claim> GetUserClaims(UserDetail user)
        {
            var domainName = user.EmailId.Split('@')[1];
            var list = new List<Claim>
            {
                new Claim("user_id", user.Id.ToString() ?? ""),
                new Claim(JwtClaimTypes.Name, (!string.IsNullOrEmpty(user.Firstname) && !string.IsNullOrEmpty(user.Lastname)) ? (user.Firstname + " " + user.Lastname) : ""),
                new Claim(JwtClaimTypes.GivenName, user.Firstname  ?? ""),
                new Claim(JwtClaimTypes.FamilyName, user.Lastname  ?? ""),
                new Claim(JwtClaimTypes.Email, user.EmailId  ?? ""),
                new Claim(Constants.IsActive, user.IsActive.ToString()),
                new Claim(Constants.IsTenant, (domainName != Constants.DefaultDomainName).ToString()),
                new Claim(Constants.DomainName, domainName),
            };

            //Custom roles
            user.Roles.ForEach(x => list.Add(new Claim(JwtClaimTypes.Role, x)));
            return list;
        }
    }
}