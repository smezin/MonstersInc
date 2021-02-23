using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonstersAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MonstersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DoorsController : ControllerBase
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private readonly IWorkDayRepository _workDayRepository;
        private readonly IDoorsRepository _doorsRepository;
        private readonly IDepletedDoorsRepository _depletedDoorsRepository;
        public DoorsController(IWorkDayRepository workDayRepository, IDoorsRepository doorsRepository, IDepletedDoorsRepository depletedDoorsRepository)
        {
            _workDayRepository = workDayRepository;
            _doorsRepository = doorsRepository;
            _depletedDoorsRepository = depletedDoorsRepository;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] bool depletedOnly = false, bool openOnly = false, bool availableOnly = false)
        {
            List<Door> content = _doorsRepository.Get()
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
        [HttpPatch]
        [Route("open")]
        public IActionResult OpenDoor([FromBody] string doorId)
        {
            log.Info($"open door requset fired by {User.FindFirstValue(ClaimTypes.GivenName)}");
            WorkDay workDay = _workDayRepository.GetActiveWorkDay(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (workDay == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response<WorkDay>
                {
                    Status = "error",
                    Message = "no workday in progress"
                });
            }

            Door door = _doorsRepository.GetDoorIfIdle(doorId);
            if (door == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response<WorkDay>
                {
                    Status = "error",
                    Message = "Invalid door id or door unavailable"
                });
            }
            _doorsRepository.OpenDoor(doorId);
            _workDayRepository.HarvestEnergy(workDay.WorkDayId, doorId);
            DepletedDoor depletedDoor = new DepletedDoor
            {
                DepletedDoorId = System.Guid.NewGuid().ToString(),
                DoorId = door.DoorId,
                WorkDayId = workDay.WorkDayId,
                OpenedAt = DateTime.Now
            };
            _doorsRepository.Put(door);
            _depletedDoorsRepository.Post(depletedDoor);
            _workDayRepository.Put(workDay);

            List<DepletedDoor> content = new List<DepletedDoor>();
            content.Add(depletedDoor);
            return Ok(new Response<DepletedDoor>
            {
                Status = "success",
                Message = "door opened",
                Content = content
            });
        }
        [HttpPatch]
        [Route("close")]
        public IActionResult CloseDoor([FromBody] string doorId)
        {
            WorkDay workDay = _workDayRepository.GetActiveWorkDay(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (workDay == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response<WorkDay>
                {
                    Status = "error",
                    Message = "no workday in progress"
                });
            }
            DepletedDoor depletedDoor = _depletedDoorsRepository.GetDepletedDoorIfOpen(doorId);
            if (depletedDoor == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response<DepletedDoor>
                {
                    Status = "error",
                    Message = "no open door found"
                });
            }

            Door door = _doorsRepository.Get()
                .Where(d => d.DoorId == doorId)
                .FirstOrDefault();
            if (door == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response<DepletedDoor>
                {
                    Status = "error",
                    Message = "caanot update door LastUsed propery. door not found"
                });
            }
            _doorsRepository.CloseDoor(doorId);
            _depletedDoorsRepository.CloseDepletedDoor(depletedDoor.DepletedDoorId);
            _doorsRepository.Put(door);
            _depletedDoorsRepository.Put(depletedDoor);
            List<DepletedDoor> content = new List<DepletedDoor>();
            content.Add(depletedDoor);
            return Ok(new Response<DepletedDoor>
            {
                Status = "success",
                Message = "door closed",
                Content = content
            });
        }

    }
}
