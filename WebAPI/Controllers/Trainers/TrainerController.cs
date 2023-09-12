using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebAPI.AppDtos;
using WebAPI.AppDtos.Enums;
using WebAPI.Controllers.Countries.Dtos;
using WebAPI.Controllers.Event.Dtos;
using WebAPI.Controllers.Trainers.Dtos;
using WebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers.Trainers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainerController : ControllerBase
    {
        private readonly TrainerService _service;
        public TrainerController(TrainerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetTrainerDto>>> Get(long? countryId)
        {
            return Ok(await _service.GetTrainers(countryId));
        }

        [HttpGet("searchTrainers")]
        public async Task<ActionResult<IEnumerable<GetTrainerDto>>> GetTrainersBySearchTearm(string? searchTerm)
        {
            return Ok(await _service.GetTrainersBySearchTerm(searchTerm));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<string>> CreateTrainer([FromBody] CreateTranierDto dto)
        {
            var result = await _service.CreateTrainer(dto);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<ServiceResponseDto>> UpdateEvent([FromQuery] long trainerId, [FromBody] CreateTranierDto dto)
        {
            var result = await _service.UpdateTrainer(trainerId, dto);
            return Ok(result);
        }

        [HttpGet("GetTrainerById")]
        public async Task<ActionResult<GetTrainerByIdDto>> GetTrainerById(long trainerId)
        {
            var result = await _service.GetTrainerById(trainerId);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> Delete(long id)
        {
            var result = await _service.DeleteTrainer(id);
            return Ok(result);
        }
    }
}
