using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonstersAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MonstersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IntimidatorController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {            
            IntimidatorCard intimidatorCard = new IntimidatorCard
            {
                UserName = User.FindFirstValue(ClaimTypes.GivenName),
                FirstName = User.FindFirstValue(ClaimTypes.Name),
                FamilyName = User.FindFirstValue(ClaimTypes.Surname),
                PhoneNumber = User.FindFirstValue(ClaimTypes.MobilePhone),
                Email = User.FindFirstValue(ClaimTypes.Email),
                
            };
            return Ok(intimidatorCard);
        }
    }
}
