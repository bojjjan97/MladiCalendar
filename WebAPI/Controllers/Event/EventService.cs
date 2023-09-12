using Humanizer;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebAPI.AppDtos;
using WebAPI.AppDtos.Enums;
using WebAPI.Controllers.Event.Dtos;
using WebAPI.Controllers.Trainers.Dtos;
using WebAPI.Data;
using WebAPI.Models;
using WebAPI.Models.Enums;
using WebAPI.StandaloneServices.EmailService;
using WebAPI.StandaloneServices.EmailService.Dtos;
using WebAPI.StandaloneServices.EmailServiceNamespace;

namespace WebAPI.Controllers.Event
{
    public class EventService
    {
        private readonly MainContext _context;
        private readonly EmailService _emailService;
        public EventService(MainContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<PaginatedResponseDto> GetAllEvents(bool upcoming, string userId, int page)
        {
            var pageLength = 10;

            if (upcoming)
            {
                var upcomingCount = _context.Events.Where(x => x.EventDateTime > DateTime.UtcNow).Count();

                var upcomingData = await _context.Events.Where(x => x.EventDateTime > DateTime.UtcNow)
                .Select(x =>
                new GetEventDto
                {
                    EventId = x.Id,
                    Name = x.Name,
                    FreePlaces = x.NumberOfParticipants > x.UsersOnEvent.Count(),
                    Location = x.Location,
                    OnlineMeetingUrl = x.OnlineMeetingUrl,
                    NumberOfParticipants = x.NumberOfParticipants,
                    NumberOfRegisteredParticipants = x.UsersOnEvent.Count(),
                    EventUserType = x.EventForUserType,
                    Description = x.Description,
                    EventDateTime = x.EventDateTime,
                    Trainers = x.TrainersOnEvent.Select(x => new GetTrainerOnEventDto { TrainerId = x.TrainerId, FirstName = x.Trainer.FirstName, LastName = x.Trainer.LastName, Email = x.Trainer.Email, Country = x.Trainer.Country }).ToList(),
                    UsersOnEvent = x.UsersOnEvent.Select(x => new GetUserOnEventDto { UserId = x.UserId, FirstName = x.User.FirstName, LastName = x.User.LastName, Email = x.User.Email, EmailSent = x.EmailSent }).ToList()
                })
                .OrderByDescending(x => x.EventDateTime)
                .Skip((page - 1) * pageLength).Take(pageLength)
                  .ToListAsync();
                return new PaginatedResponseDto { Data = upcomingData, Page = page, Total = upcomingCount, TotalCertificatesSent = 0 };
            }
            var count = _context.Events.Where(x => x.EventDateTime < DateTime.UtcNow).Count();
            var totalCertificatesSent = _context.UsersOnEvents.Where(x => x.EmailSent == true).Count();

            var data = await _context.Events.Where(x => x.EventDateTime < DateTime.UtcNow)
                    .Select(x =>
                    new GetEventDto
                    {
                        EventId = x.Id,
                        Name = x.Name,
                        FreePlaces = x.NumberOfParticipants > x.UsersOnEvent.Count(),
                        Location = x.Location,
                        OnlineMeetingUrl = x.OnlineMeetingUrl,
                        NumberOfParticipants = x.NumberOfParticipants,
                        NumberOfRegisteredParticipants = x.UsersOnEvent.Count(),
                        EventUserType = x.EventForUserType,
                        Description = x.Description,
                        EventDateTime = x.EventDateTime,
                        Trainers = x.TrainersOnEvent.Select(x => new GetTrainerOnEventDto { TrainerId = x.TrainerId, FirstName = x.Trainer.FirstName, LastName = x.Trainer.LastName, Email = x.Trainer.Email, Country = x.Trainer.Country }).ToList(),
                        UsersOnEvent = x.UsersOnEvent.Select(x => new GetUserOnEventDto { UserId = x.UserId, FirstName = x.User.FirstName, LastName = x.User.LastName, Email = x.User.Email, EmailSent = x.EmailSent }).ToList()
                    })
                    .OrderByDescending(x=>x.EventDateTime)
                    .Skip((page - 1) * pageLength).Take(pageLength)
                    .ToListAsync();
            return new PaginatedResponseDto { Data = data,Page = page,Total = count, TotalCertificatesSent = totalCertificatesSent };

        }

        public async Task<IEnumerable<GetEventDto>> GetEvents(string From, string To,string userId)
        {
            var user = this._context.Users.FirstOrDefault(x => x.Id == userId);
            
            var dateFrom = DateTime.Parse(From);
            var dateTo = DateTime.Parse(To);
            var events = _context.Events
                .Include(x => x.UsersOnEvent)
                .Include(x => x.TrainersOnEvent)
                .ThenInclude(x => x.Trainer)
                .AsNoTracking()
                .Where(x => x.EventDateTime >= dateFrom && x.EventDateTime <= dateTo)

                .Select(x =>
                    new GetEventDto
                    {
                        EventId = x.Id,
                        Name = x.Name,
                        FreePlaces = x.NumberOfParticipants > x.UsersOnEvent.Count(),
                        Location = x.Location,
                        OnlineMeetingUrl = x.OnlineMeetingUrl,
                        NumberOfParticipants = x.NumberOfParticipants,
                        NumberOfRegisteredParticipants = x.UsersOnEvent.Count(),
                        EventUserType = x.EventForUserType,
                        Description = x.Description,
                        EventDateTime = x.EventDateTime,
                        CurrentUserAlreadyOnEvent = x.UsersOnEvent.Any(x => x.UserId == userId),
                        Trainers = x.TrainersOnEvent.Select(x => new GetTrainerOnEventDto { TrainerId = x.TrainerId,  FirstName = x.Trainer.FirstName, LastName = x.Trainer.LastName, Email = x.Trainer.Email, Country = x.Trainer.Country }).ToList(),
                        UsersOnEvent = x.UsersOnEvent.Select(x => new GetUserOnEventDto { UserId = x.UserId, FirstName = x.User.FirstName, LastName = x.User.LastName, Email = x.User.Email, EmailSent = x.EmailSent, Country = x.User.Country }).ToList(),
                    });

            if (new List<long> { (long)EUserType.HighschoolStudent, (long)EUserType.Student }
            .Contains((long)user.UserType)){
                events.Where(x => x.EventUserType == user.UserType);
            }

             var res = await events.ToListAsync();
            return res;
        }

        public async Task<GetEventByIdDto> GetEventById(long eventId)
        {
            return await _context.Events.Where(x => x.Id == eventId)
            .Select(x =>
                    new GetEventByIdDto
                    {
                        EventId = x.Id,
                        Name = x.Name,
                        FreePlaces = x.NumberOfParticipants > x.UsersOnEvent.Count(),
                        Location = x.Location,
                        OnlineMeetingUrl = x.OnlineMeetingUrl,
                        NumberOfParticipants = x.NumberOfParticipants,
                        NumberOfRegisteredParticipants = x.UsersOnEvent.Count(),
                        EventUserType = x.EventForUserType,
                        Description = x.Description,
                        EventDateTime = x.EventDateTime,
                        UsersOnEvent = x.UsersOnEvent.Select(x => new GetUserOnEventDto { UserId = x.UserId, FirstName = x.User.FirstName, LastName = x.User.LastName, Email = x.User.Email, EmailSent = x.EmailSent }).ToList(),
                        Trainers = x.TrainersOnEvent.Select(x => x.Trainer.FirstName).ToList()
                    }).FirstOrDefaultAsync();
        }

        public async Task<ServiceResponseDto> UnregisterUserFromEvent(string UserId, long EventId)
        {
            await _context.UsersOnEvents.Where(x=>x.UserId == UserId && x.EventId == EventId).ExecuteDeleteAsync();
            if (await _context.SaveChangesAsync() != -1)
                return new ServiceResponseDto { ResponseType = EResponseType.Success, Message = "User succsessfully removed from event." };
            return new ServiceResponseDto { ResponseType = EResponseType.Error, Message = "Error while processing request." };
        }

        public async Task<ServiceResponseDto> CreateEvent(CreateEventDto dto)
        {
            
            await _context.Events.AddAsync(new Models.Event
            {
                Location = dto.Location,
                Name = dto.Name,
                NumberOfParticipants = dto.NumberOfParticipants,
                OnlineMeetingUrl = dto.OnlineMeetingUrl,
                EventDateTime = dto.EventDateTime,
                Description = dto.Description,
                EventForUserType = dto.EventForUserType,
                TrainersOnEvent = dto.TrainerIds.Select(x => new TrainerOnEvent { TrainerId = x}).ToList()
                
            });
            if (await _context.SaveChangesAsync() > 0)
                return new ServiceResponseDto { ResponseType = EResponseType.Success, Message = "Event succsessfully created."};
            return new ServiceResponseDto { ResponseType = EResponseType.Error, Message = "Error while processing request." };
        }

        public async Task<ServiceResponseDto> AddUserOnEvent(string UserId, long EventId)
        {
            var ev = await _context.Events.Include(x=>x.UsersOnEvent).ThenInclude(x=>x.User).FirstAsync(x => x.Id == EventId);
            var user = await _context.Users.FirstAsync(x => x.Id == UserId);

            
            
            if (ev.NumberOfParticipants == ev.UsersOnEvent.Count())
                return new ServiceResponseDto { Message = "Event is full", ResponseType = EResponseType.Error };

            //Provjera država koje ima registrovane učesnike.

            List<int> countries = await _context.Countries.Select(x => x.Id).ToListAsync();

            var countriesWithParticipants = countries.Select(c => new
            {
                CountryId = c,
                AnyUser = ev.UsersOnEvent.Any(x=>x.User.CountryId == c)
            });
            
            if(ev.NumberOfParticipants - ev.UsersOnEvent.Count() <= countriesWithParticipants.Where(x=>!x.AnyUser).Count()
                && !countriesWithParticipants.Where(x => !x.AnyUser).Select(x=>x.CountryId).ToList().Contains(user.CountryId))
                return new ServiceResponseDto { Message = "Event is full", ResponseType = EResponseType.Error };


            await _context.UsersOnEvents.AddAsync(new Models.UserOnEvent { UserId = UserId, EventId = EventId });   
            if(await _context.SaveChangesAsync() > 0)
                return new ServiceResponseDto { ResponseType = EResponseType.Success, Message = "User succsessfully added to event." };
            return new ServiceResponseDto { ResponseType = EResponseType.Error, Message = "Error while processing request." };
        }

        public async Task<ServiceResponseDto> AddUserOnEventFromEmail(string email,long EventId)
        {
            var ev = await _context.Events.Include(x => x.UsersOnEvent).FirstAsync(x => x.Id == EventId);
           
            return null;
        }

        public async Task<ServiceResponseDto> UpdateEvent(long eventId,CreateEventDto dto)
        {
             await _context.Events
                .Where(x => x.Id == eventId)
                .ExecuteUpdateAsync(
                    setters => setters
                        .SetProperty(b => b.Location, dto.Location)
                        .SetProperty(b => b.Name, dto.Name)
                        .SetProperty(b => b.NumberOfParticipants, dto.NumberOfParticipants)
                        .SetProperty(b => b.OnlineMeetingUrl, dto.OnlineMeetingUrl)
                        .SetProperty(b => b.EventDateTime, dto.EventDateTime)
                        .SetProperty(b => b.Description, dto.Description)
                        .SetProperty(b => b.EventForUserType, dto.EventForUserType)
                    );
            var ev = _context.Events.Where(x => x.Id == eventId).First();
            await _context.TrainerOnEvents.Where(x => x.EventId == eventId).ExecuteDeleteAsync();
             ev.TrainersOnEvent = dto.TrainerIds.Select(x => new TrainerOnEvent { TrainerId = x }).ToList();
            if (await _context.SaveChangesAsync() != -1)
                return new ServiceResponseDto { ResponseType = EResponseType.Success, Message = "Event succsessfully updated.",ResponseObject = dto };
            return new ServiceResponseDto { ResponseType = EResponseType.Error, Message = "Error while processing request." };
        }

        public async Task<ServiceResponseDto> DeleteEvent(long eventId)
        {
            await _context.Events.Where(x => x.Id == eventId).ExecuteDeleteAsync();
            if (await _context.SaveChangesAsync() != -1)
                return new ServiceResponseDto { ResponseType = EResponseType.Success, Message = "Event succsessfully deleted." };
            return new ServiceResponseDto { ResponseType = EResponseType.Error, Message = "Error while processing request." };
        }

        public async Task<ServiceResponseDto> SendCertificates(long eventId, string[] userIds)
        {
            //.Where(user => user.UserId in userIds)
            var ev = await _context.Events
                .Include(x => x.UsersOnEvent)
                .ThenInclude(x=>x.User)
                .FirstOrDefaultAsync(x => x.Id == eventId);

            if (ev == null)
                return new ServiceResponseDto { Message = "Event with reacived identifier doesnt exist", ResponseType = EResponseType.Error };

            var certificateData = ev.UsersOnEvent.Where(uoe => userIds.Contains(uoe.UserId)).Select(x => new
            {
                Email = x.User.Email,
                FullName = $"{x.User.FirstName} {x.User.LastName}",
                AttendedEvent = ev.Name
            })
                .ToList();
            try
            {
                certificateData.ForEach(async x => {
                    var mailRequest = new MailRequest {
                        ToEmail = x.Email,
                        Subject = "Certificate",
                        Body = "asdf"
                    };
                    Thread.Sleep(500);
                    await _emailService.SendEmailAsync(mailRequest);
                });
                return new ServiceResponseDto { Message = "Success", ResponseType = EResponseType.Success };
            }
            catch
            {
                return new ServiceResponseDto { Message = "Error", ResponseType = EResponseType.Error };
            }

        }
    }
}
