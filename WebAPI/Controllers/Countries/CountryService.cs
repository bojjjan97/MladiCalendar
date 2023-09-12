using Microsoft.EntityFrameworkCore;
using WebAPI.AppDtos;
using WebAPI.AppDtos.Enums;
using WebAPI.Controllers.Countries.Dtos;
using WebAPI.Controllers.Event.Dtos;
using WebAPI.Data;
using WebAPI.Models;
using WebAPI.StandaloneServices.EmailService;

namespace WebAPI.Controllers.Countries
{
    public class CountryService
    {
        private readonly MainContext _context;

        public CountryService(MainContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<GetCountryDto>> GetCoutnries()
        {
            return await _context.Countries
                .AsNoTracking()
                .Select(x =>
                    new GetCountryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                    }).ToListAsync();
        }

        public async Task<ServiceResponseDto> CreateCoutnry(CreateCountryDto dto)
        {
            await _context.Countries.AddAsync(new Country
            {
                Name = dto.Name
            });
            var response = await _context.SaveChangesAsync();
            if (response != -1)
                return new ServiceResponseDto { ResponseType = EResponseType.Success, Message = "Country succsessfuly created", ResponseObject = dto };
            return new ServiceResponseDto { ResponseType = EResponseType.Error, Message = "Error occured while creating country" };
        }

        public async Task<ServiceResponseDto> DeleteCoutnry(long countryId)
        {
            await _context.Countries.Where(x => x.Id == countryId).ExecuteDeleteAsync();
            var response = await _context.SaveChangesAsync();
            if (response != -1)
                return new ServiceResponseDto { ResponseType = EResponseType.Success, Message = "Country succsessfuly deleted" };
            return new ServiceResponseDto { ResponseType = EResponseType.Error, Message = "Error occured while deleting country" };
        }
    }
}
