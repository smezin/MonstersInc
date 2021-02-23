using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace IdentityServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class IdentityController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public IdentityController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpPost]
        [Route("register")]
      
        public async Task<IActionResult> Register ([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (await userManager.FindByEmailAsync(model.Email) != null ||
                    await userManager.FindByNameAsync(model.UserName) != null) 
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "username or email already exists");
                }
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    FamilyName = model.FamilyName,
                    Tentacles = model.Tentacles,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    ScaringSince = DateTime.Now
                };
                user.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user, model.Password);
                await userManager.CreateAsync(user, model.Password);
                return Ok(user);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }
    }
}
