using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ResourceOwnerPasswordValidator> _logger;

        public ResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, ILogger<ResourceOwnerPasswordValidator> logger)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
            _logger = logger;
        }
        
        //this is used to validate your user account with provided grant at /connect/token
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                //get your user model from db 

                var user = await _userManager.FindByNameAsync(context.UserName);
                if (user != null)
                {
                    //check if password match
                    if (await _userManager.CheckPasswordAsync(user, context.Password))
                    {
                        var principal = await _claimsFactory.CreateAsync(user);
                        var claims = principal.Claims.ToList();
                        claims.Add(new Claim(JwtClaimTypes.GivenName, user.UserName));                   
                        claims.Add(new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber));                    
                        claims.Add(new Claim("Tentacles", user.Tentacles.ToString()));
                        claims.Add(new Claim(JwtClaimTypes.Role, "admin"));
                        claims.Add(new Claim(IdentityServerConstants.StandardScopes.Email, user.Email));

                        //set the result
                        context.Result = new GrantValidationResult(
                            subject: user.Id.ToString(),
                            authenticationMethod: "custom",
                            claims: claims);

                        return;
                    }

                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Incorrect password");
                    return;
                }
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "User does not exist.");
                return;
            }
            catch (Exception ex)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, $"Invalid username or password {ex}");
            }
        }
    }
}
