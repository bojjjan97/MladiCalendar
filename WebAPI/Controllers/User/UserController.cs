using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPI.Controllers.Trainers.Dtos;

namespace WebAPI.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;
        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpGet("searchUsers")]
        public async Task<ActionResult<IEnumerable<GetTrainerDto>>> GetUsersBySearchTearm(string? searchTerm)
        {
            return Ok(await _service.GetUserBySearchTerm(searchTerm));
        }

        [HttpGet("GetUserById")]
        public async Task<ActionResult<IEnumerable<GetTrainerDto>>> GetUserById()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(await _service.GetUserById(userId));
        }
    }
}
