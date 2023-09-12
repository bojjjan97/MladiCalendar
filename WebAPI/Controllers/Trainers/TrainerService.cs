using Microsoft.EntityFrameworkCore;
using WebAPI.AppDtos.Enums;
using WebAPI.AppDtos;
using WebAPI.Controllers.Trainers.Dtos;
using WebAPI.Data;
using WebAPI.Models;
using NuGet.DependencyResolver;

namespace WebAPI.Controllers.Trainers
{
    public class TrainerService
    {
        private readonly MainContext _context;

        public TrainerService(MainContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<GetTrainerDto>> GetTrainers(long? countryId)
        {
            IQueryable<Trainer> trainers = _context.Set<Trainer>().Include(x=>x.Country);
            if (countryId != null)
                trainers = trainers.Where(x => x.CountryId == countryId);
            return await trainers
                .OrderBy(x => x.LastName)
                .ThenBy(x=>x.FirstName)
                .Select(x =>
               new GetTrainerDto
                    {
                        Id = x.Id,
                        FullName = $"{x.LastName} {x.FirstName}",
                        PhoneNumber = x.PhoneNumber,
                        Email = x.Email,
                        Country = x.Country.Name
                    })
                
                .ToListAsync();
        }

        public async Task<ServiceResponseDto> CreateTrainer(CreateTranierDto dto)
        {
            var trainer = new Trainer
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                CountryId = dto.CountryId
            };
            await _context.Trainers.AddAsync(trainer);
            var response = await _context.SaveChangesAsync();
            if (response != -1)
                return new ServiceResponseDto { ResponseType = EResponseType.Success, Message = "Trainer succsessfuly created", ResponseObject = trainer };
            return new ServiceResponseDto { ResponseType = EResponseType.Error, Message = "Error occured while creating trainer" };
        }

        public async Task<ServiceResponseDto> DeleteTrainer(long trainerId)
        {
            await _context.TrainerOnEvents.Where(x => x.TrainerId == trainerId).ExecuteDeleteAsync();
            await _context.Trainers.Where(x => x.Id == trainerId).ExecuteDeleteAsync();
            var response = await _context.SaveChangesAsync();
            if (response != -1)
                return new ServiceResponseDto { ResponseType = EResponseType.Success, Message = "Trainer succsessfuly deleted" };
            return new ServiceResponseDto { ResponseType = EResponseType.Error, Message = "Error occured while deleting trainer" };
        }

        public async Task<ServiceResponseDto> UpdateTrainer(long trainerId, CreateTranierDto dto)
        {
              await _context.Trainers
               .Where(x => x.Id == trainerId)
               .ExecuteUpdateAsync(
                   setters => setters
                       .SetProperty(b => b.FirstName, dto.FirstName)
                       .SetProperty(b => b.CountryId, dto.CountryId)
                       .SetProperty(b => b.LastName, dto.LastName)
                       .SetProperty(b => b.Email, dto.Email)
                       .SetProperty(b => b.PhoneNumber, dto.PhoneNumber)
                   );
            if (await _context.SaveChangesAsync() != -1)
            {
                return new ServiceResponseDto { ResponseType = EResponseType.Success, Message = "Trainer succsessfully updated." };
            }
            return new ServiceResponseDto { ResponseType = EResponseType.Error, Message = "Error while updating trainer data." };
        }

        public async Task<IEnumerable<GetTrainerDto>> GetTrainersBySearchTerm(string? st)
        {
            var searchTerm = st != null ? st.ToLower() : null;
            IQueryable<Trainer> trainers = _context.Set<Trainer>();
            if (searchTerm != null)
                trainers = trainers.Where(x => x.FirstName.ToLower().Contains(searchTerm) || x.LastName.ToLower().Contains(searchTerm) || x.Email.ToLower().Contains(searchTerm));
            return await trainers.Select(x =>
               new GetTrainerDto
               {
                   Id = x.Id,
                   FullName = $"{x.FirstName} {x.LastName}",
                   PhoneNumber = x.PhoneNumber,
                   Email = x.Email
               }).ToListAsync();
        }

        public async Task<GetTrainerByIdDto> GetTrainerById(long trainerId)
        {
            return await _context.Trainers.Where(x => x.Id == trainerId)
            .Select(x => new GetTrainerByIdDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Country = new Countries.Dtos.GetCountryDto
                {
                    Name = x.Country.Name,
                    Id = x.Country.Id,
                }
            }).FirstOrDefaultAsync();
        }
    }
}
