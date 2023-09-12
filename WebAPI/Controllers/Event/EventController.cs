using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Security.Claims;
using WebAPI.AppDtos;
using WebAPI.AppDtos.Enums;
using WebAPI.Controllers.Event.Dtos;
using WebAPI.Controllers.Trainers.Dtos;
using WebAPI.StandaloneServices.CertificateBuilderService;

namespace WebAPI.Controllers.Event
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly EventService _service;
        private CertificateBuilderService _certService;
        public EventController(EventService service,CertificateBuilderService certBuilder)
        {
            _service = service;
            this._certService = certBuilder;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("SendCertificates")]
        public async  Task<IActionResult> SendCertificates(long eventId, string[] userIds)
        {
            return Ok(await this._certService.SendCertificate(eventId, userIds));
        }

        [Route("api/allevents")]
        [HttpGet]
        public async Task<ActionResult<PaginatedResponseDto>> GetAllEvents(bool upcoming,int page)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(await _service.GetAllEvents(upcoming, userId,page));
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetEventDto>>> GetEvents(string From,string To)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(await _service.GetEvents(From,To,userId));
        }

        [HttpGet("GetEventById")]
        public async Task<ActionResult<GetEventByIdDto>> GetEventById(long eventId)
        {
            var result = await _service.GetEventById(eventId);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CreateEvent")]
        public async Task<ActionResult> CreateEvent(CreateEventDto dto)
        {
            var result = await _service.CreateEvent(dto);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("AddUserOnEvent")]
        public async Task<ActionResult<ServiceResponseDto>> AddUserOnEvent(long eventId)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _service.AddUserOnEvent(userId, eventId);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("UnregisterUserFromEvent")]
        public async Task<ActionResult<ServiceResponseDto>> UnregisterUserFromEvent(long eventId)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _service.UnregisterUserFromEvent(userId, eventId);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<ServiceResponseDto>> UpdateEvent([FromQuery]long eventId,[FromBody] CreateEventDto dto)
        {
            var result = await _service.UpdateEvent(eventId, dto);
            if (result.ResponseType == EResponseType.Success)
                return Ok(result.ResponseObject);
            return BadRequest(result.Message);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<ActionResult<ServiceResponseDto>> DeleteEvent(long eventId)
        {
            var result = await _service.DeleteEvent(eventId);
            if (result.ResponseType == EResponseType.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
