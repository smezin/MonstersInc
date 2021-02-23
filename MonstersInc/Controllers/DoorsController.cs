using MonstersAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonstersAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MonstersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DoorsController : ControllerBase
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private readonly IDoorsRepository _repository;
        public DoorsController(IDoorsRepository repository)
        {
            _repository = repository;
        }
       
        [HttpGet]
        public IActionResult Get([FromQuery] bool depletedOnly = false, bool openOnly = false, bool availableOnly = false)
        {
            List<Door> content = _repository.Doors
                .Where(d => d.LastUsed.Date == DateTime.Today.Date || !depletedOnly) 
                .Where(d => d.IsOpen == true || !openOnly)
                .Where(d => (d.IsOpen == false && d.LastUsed.Date != DateTime.Today.Date) || !availableOnly)
                .ToList();
            log.Info($"get doors request fired by {User.FindFirstValue(ClaimTypes.GivenName)}");
            return Ok(new Response<Door> 
            {
                Status = "success",
                Message = "Get doors success",
                Content = content
            });
        }

    }
}
