using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer
{
    public class SeedData
    {
        public void Seed(List<ApiResource> apiResourceOptions, List<Client> clientOptions, ConfigurationDbContext configurationDbContext, AppDbContext appDbContext)
        {
            SeedIdentityUsersDetails(appDbContext);
            SeedConfigurationData(apiResourceOptions, clientOptions, configurationDbContext);
        }

        private void SeedIdentityUsersDetails(AppDbContext appDbContext)
        {
            SeedIdentityRoles(appDbContext);
            SeedIdentityUsers(appDbContext);
            SeedIdentityUserRoles(appDbContext);
        }

        private void SeedIdentityRoles(AppDbContext appDbContext)
        {
            if (!appDbContext.Roles.Any())
            {
                List<IdentityRole> roles = new List<IdentityRole>();
                roles.Add(new IdentityRole()
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "Admin"
                });

                roles.Add(new IdentityRole()
                {
                    Id = "2",
                    Name = "User",
                    NormalizedName = "User"
                });

                foreach (var role in roles)
                {
                    appDbContext.Roles.Add(role);
                }

                appDbContext.SaveChanges();
            }
        }

        private void SeedIdentityUsers(AppDbContext appDbContext)
        {
            if (!appDbContext.Users.Any())
            {
                List<ApplicationUser> users = new List<ApplicationUser>();

                var adminUser = new ApplicationUser()
                {
                    Id = "1",
                    UserName = "admin",
                    NormalizedUserName = "admin",
                    Email = "admin@admin.com",
                    NormalizedEmail = "admin@admin.com",
                    EmailConfirmed = true,
                };
                adminUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(adminUser, "secret");
                users.Add(adminUser);

                var userUser = new ApplicationUser()
                {
                    Id = "2",
                    UserName = "user",
                    NormalizedUserName = "user",
                    Email = "user@user.com",
                    NormalizedEmail= "user@user.com",
                    EmailConfirmed = true
                };
                userUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(userUser, "secret");
                users.Add(userUser);

                foreach (var user in users)
                {
                    appDbContext.Users.Add(user);
                }

                appDbContext.SaveChanges();
            }
        }

        private void SeedIdentityUserRoles(AppDbContext appDbContext)
        {
            if (!appDbContext.UserRoles.Any())
            {
                List<IdentityUserRole<string>> userRoles = new List<IdentityUserRole<string>>();
                userRoles.Add(new IdentityUserRole<string>()
                {
                    UserId = "1",
                    RoleId = "1"
                });

                userRoles.Add(new IdentityUserRole<string>()
                {
                    UserId = "2",
                    RoleId = "2"
                });

                foreach (var userRole in userRoles)
                {
                    appDbContext.UserRoles.Add(userRole);
                }

                appDbContext.SaveChanges();
            }
        }

        private void SeedConfigurationData(List<ApiResource> apiResourceOptions, List<Client> clientOptions, ConfigurationDbContext configurationDbContext)
        {
            IdentityConfig identityConfig = new IdentityConfig();

            if (!configurationDbContext.Clients.Any())
            {
                foreach (var client in identityConfig.GetClients(clientOptions))
                {
                    configurationDbContext.Clients.Add(client.ToEntity());
                }
                configurationDbContext.SaveChanges();
            }

            if (!configurationDbContext.IdentityResources.Any())
            {
                foreach (var resource in identityConfig.GetIdentityResources())
                {
                    configurationDbContext.IdentityResources.Add(resource.ToEntity());
                }
                configurationDbContext.SaveChanges();
            }

            if (!configurationDbContext.ApiResources.Any())
            {
                foreach (var resource in identityConfig.GetApiResources(apiResourceOptions))
                {
                    configurationDbContext.ApiResources.Add(resource.ToEntity());
                }
                configurationDbContext.SaveChanges();
            }
        }
    }
}
