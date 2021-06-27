using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer4Demo
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource(Constants.ApiResourceName, Constants.ApiResourceDisplayName, new[]
                {
                    JwtClaimTypes.Email ,
                    JwtClaimTypes.FamilyName ,
                    JwtClaimTypes.GivenName ,
                    JwtClaimTypes.NickName ,
                    JwtClaimTypes.PreferredUserName ,
                    JwtClaimTypes.Role ,
                    JwtClaimTypes.Name,
                    Constants.IsActive,
                    Constants.IsTenant,
                    Constants.DomainName
                })
                {
                    ApiSecrets = { new Secret(Constants.ApiSecrets.Sha256()) }
                },

                // new ApiResource(Constants.ApiResourceName, Constants.ApiResourceDisplayName)
             };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                // ClientCredentials
                new Client
                {
                    ClientId = Constants.ClientCredentials_ClientId_m2m,
                    ClientName = "Machine to machine (client credentials)",
                    ClientSecrets = { new Secret(Constants.ClientSecret.Sha256()) },
                    AlwaysSendClientClaims = true,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { Constants.ApiResourceName },
                },

                // Password
                new Client
                {
                    ClientId = Constants.PasswordFlow_ClientId,
                    ClientSecrets = { new Secret(Constants.ClientSecret.Sha256()) },
                    AlwaysSendClientClaims = true,

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    RequireConsent = false,
                    RequirePkce = true,

                    AllowedScopes = new List<string>
                    {
                        Constants.ApiResourceName,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Profile
                    },

                    AllowOfflineAccess = true
                },

                // Code
                new Client
                {
                    ClientId = Constants.ClientCodeFlow_ClientId,
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    RequireClientSecret = false,
                    RequireConsent = false,
                    AlwaysSendClientClaims = true,

                    AllowedGrantTypes = GrantTypes.Code,
                
                    // where to redirect to after login
                    RedirectUris = { "http://localhost:5002/signin-oidc" },
                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        Constants.ApiResourceName
                    },

                    AllowOfflineAccess = true
                }
            };
        }
    }
}
