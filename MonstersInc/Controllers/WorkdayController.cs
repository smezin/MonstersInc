using MonstersAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace MonstersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkdayController : ControllerBase
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private readonly IWorkDayRepository _workDayRepository;
        private readonly IDoorsRepository _doorsRepository;
        private readonly IDepletedDoorsRepository _depletedDoorsRepository;
        public WorkdayController (IWorkDayRepository workDayRepository, IDoorsRepository doorsRepository, IDepletedDoorsRepository depletedDoorsRepository)
        {
            _workDayRepository = workDayRepository;
            _doorsRepository = doorsRepository;
            _depletedDoorsRepository = depletedDoorsRepository;
        }
        [HttpGet]
        public IActionResult Get ([FromQuery] bool onlyGoalAccomplished, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {            
            log.Info($"get workday request fired by {User.FindFirstValue(ClaimTypes.GivenName)}");
            List<WorkDay> content = _workDayRepository.WorkDays
                .Where(w => w.IntimidatorId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                .Where(w => w.Begin > from || from == DateTime.MinValue.Date)
                .Where(w => w.End < to || to == DateTime.MinValue.Date)
                .Where(w => w.EnergyCollected >= w.EnergyGoal || !onlyGoalAccomplished)
                .Include(w => w.DepletedDoors)
                .ToList();
            return Ok(new Response<WorkDay>
            {
                Status = "success",
                Message = "list generated",
                Content = content
            });
        }
        [HttpPost]
        [Route("begin")]
        public IActionResult Begin ()
        {
            log.Info($"begin workday requset fired by {User.FindFirstValue(ClaimTypes.GivenName)}");
            try
            {
                List<WorkDay> content = new List<WorkDay>();
                WorkDay activeWorkDay = _workDayRepository.WorkDays
                    .Where(w => w.IntimidatorId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                    .Where(w => w.End == DateTime.MinValue)
                    .FirstOrDefault();
                if (activeWorkDay != null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new Response<WorkDay>
                    {
                        Status = "error",
                        Message = "workday already in progress"
                    });
                }
                WorkDay workDay = new WorkDay
                {
                    WorkDayId = System.Guid.NewGuid().ToString(),
                    Begin = DateTime.Now,
                    IntimidatorId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    EnergyGoal = 100
                };
                content.Add(workDay);
                _workDayRepository.CreateWorkDay(workDay);
                return Ok(new Response<WorkDay>
                {
                    Status = "success",
                    Message = "workday created",
                    Content = content
                });
            }
            catch (Exception e)
            {
                log.Debug($"Exception: {e}");
                return StatusCode(StatusCodes.Status403Forbidden, new Response<WorkDay>
                {
                    Status = "error",
                    Message = $"exeption {e}"
                });
            }         
        }
        [HttpPatch]
        [Route("end")]
        public IActionResult End()
        {
            log.Info($"end workday requset fired by {User.FindFirstValue(ClaimTypes.GivenName)}");
            try
            {
                List<WorkDay> content = new List<WorkDay>();
                WorkDay workDay = GetActiveWorkDay(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (workDay != null)
                {
                    workDay.End = DateTime.Now;
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new Response<WorkDay>
                    {
                        Status = "error",
                        Message = "no active workday to end"
                    });
                }
                content.Add(workDay);
                _workDayRepository.PatchWorkDay(workDay);
                return Ok(new Response<WorkDay>
                {
                    Status = "success",
                    Message = "workday ended",
                    Content = content
                });
            }
            catch (Exception e)
            {
                log.Debug($"Exception {e}");
                return StatusCode(StatusCodes.Status403Forbidden, new Response<WorkDay>
                {
                    Status = "error",
                    Message = $"exeption {e}"
                });
            }
        }

        //helpers
        private WorkDay GetActiveWorkDay(string intimidatorId)
        {
            return _workDayRepository.WorkDays
                .Where(w => w.IntimidatorId == intimidatorId)
                .Where(w => w.End == DateTime.MinValue)
                .FirstOrDefault();
        }
    }
}
