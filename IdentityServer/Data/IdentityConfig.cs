using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer
{ 
    public class IdentityConfig
    {        
        public IEnumerable<ApiResource> GetApiResources(List<ApiResource> apiResources)
        {
            if (apiResources == null || apiResources.Count == 0)
                apiResources = new List<ApiResource>
                {                    
                    new ApiResource("monstersapi", "Monsters Inc API")
                    {
                        Scopes = {new Scope("monstersapiscope") },
                        UserClaims = new List<string> {"role"}
                    }
                };
            return apiResources;
        }

        public IEnumerable<Client> GetClients(List<Client> clients)
        {
            if (clients == null || clients.Count == 0)
                clients = new List<Client>
                {
                    new Client
                    {
                        ClientId = "web_client_id",
                        AllowedGrantTypes =
                        {
                            GrantType.ResourceOwnerPassword
                        },
                        ClientSecrets =
                        {
                            new IdentityServer4.Models.Secret("websecret".Sha256())
                        },
                        AllowedScopes = {
                            "monstersapiscope", "openid",
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Email
                        },
                        AccessTokenLifetime = 60*60*24
                    }
                };
            return clients;
        }

        internal IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId()               
                ,new IdentityResources.Email()
                ,new IdentityResource
                {
                    Name = "role",
                    UserClaims = new List<string> {"role"}
                }
            };
        }
    }
}
