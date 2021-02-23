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
                            GrantType.ResourceOwnerPassword,                     
                        },
                        ClientSecrets =
                        {
                            new IdentityServer4.Models.Secret("websecret".Sha256())
                        },
                        AllowedScopes = { 
                            "monstersapiscope",
                    //        IdentityServerConstants.StandardScopes.OpenId,
                    //        IdentityServerConstants.StandardScopes.Profile
                        },
                    }
                };

            return clients;
        }

        public List<TestUser> GetTestUsers(List<TestUser> testUsers)
        {
            if (testUsers == null || testUsers.Count == 0)
                testUsers = new List<TestUser>
                {
                    new TestUser
                    {
                        SubjectId = "1",
                        Username = "admin",
                        Password = "admin",
                        Claims = new List<Claim> {
                            new Claim(JwtClaimTypes.Email, "admin@admin.com"),
                            new Claim(JwtClaimTypes.Role, "admin")
                        }
                    },
                    new TestUser
                    {
                        SubjectId = "1",
                        Username = "user",
                        Password = "user",
                        Claims = new List<Claim> {
                            new Claim(JwtClaimTypes.Email, "user@user.com"),
                            new Claim(JwtClaimTypes.Role, "user")
                        }
                    }
                };

            foreach (var user in testUsers)
            {
                if (user.Claims == null || user.Claims.Count == 0)
                {
                    if (user.SubjectId == "1")
                    {
                        user.Claims = new List<Claim> {
                            new Claim(JwtClaimTypes.Email, "admin@admin.com"),
                            new Claim(JwtClaimTypes.Role, "admin")
                        };
                    }

                    if (user.SubjectId == "2")
                    {
                        user.Claims = new List<Claim> {
                            new Claim(JwtClaimTypes.Email, "user@user.com"),
                            new Claim(JwtClaimTypes.Role, "user")
                        };
                    }
                }
            }

            return testUsers;
        }

        internal IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId()
                //,new IdentityResources.Profile()
               // ,new IdentityResources.Email()
                ,new IdentityResource
                {
                    Name = "role",
                    UserClaims = new List<string> {"role"}
                }
            };
        }
    }
}
