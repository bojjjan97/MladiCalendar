using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPI.AppDtos.Enums;
using WebAPI.Controllers.Auth.Dtos;

namespace WebAPI.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _service;
        public AuthController(AuthService service)
        {
            _service = service;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var serviceResponse = await _service.Login(dto);
            return Ok(serviceResponse);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Login([FromBody] RegisterDto dto)
        {
            var serviceResponse = await _service.Register(dto, false);
                return Ok(serviceResponse);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDto dto)
        {
            dto.UserType = Models.Enums.EUserType.Admin;
            var serviceResponse = await _service.Register(dto, true);
            return Ok(serviceResponse);
        }

        [HttpPost("ChangeUserData")]
        public async Task<IActionResult> ChangeUserData(ChangeUserDto dto)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var res = await _service.ChangeUserData(dto, userId);
            return Ok(res);
        }
    }
}
