using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data;
using WebAPI.AppDtos.Enums;
using WebAPI.Controllers.Countries.Dtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers.Countries
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly CountryService _service;
        public CountryController(CountryService service)
        {
            _service = service;
        }
        // GET: api/<CountryController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> Get()
        {
            return Ok(await _service.GetCoutnries());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<string>> CreateCountry([FromBody] CreateCountryDto dto)
        {
            var result = await _service.CreateCoutnry(dto);
            if (result.ResponseType == EResponseType.Success)
                return Ok(result.Message);
            return BadRequest(result.Message);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> Delete(long id)
        {
            var result = await _service.DeleteCoutnry(id);
            if (result.ResponseType == EResponseType.Success)
                return Ok(result.Message);
            return BadRequest(result.Message);
        }
    }
}
