using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebAPI.Controllers.Event.Dtos;
using WebAPI.Controllers.Trainers.Dtos;
using WebAPI.Controllers.User.Dtos;
using WebAPI.Data;

namespace WebAPI.Controllers.User
{
    public class UserService
    {
        private readonly MainContext _context;

        public UserService(MainContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<UserDto>> GetUserBySearchTerm(string? st)
        {
            var searchTerm = st != null ? st.ToLower() : null;
            IQueryable<Models.User> users = _context.Set<Models.User>();
            if (searchTerm != null)
                users = users.Where(x => x.FirstName.ToLower().Contains(searchTerm) || x.LastName.ToLower().Contains(searchTerm) || x.Email.ToLower().Contains(searchTerm));
            return await users.Select(x =>
               new UserDto
               {
                   Id = x.Id,
                   FullName = $"{x.FirstName} {x.LastName}",
                   PhoneNumber = x.PhoneNumber,
                   Email = x.Email
               }).ToListAsync();
        }

        public async Task<GetByIdDto> GetUserById(string userId)
        {
            return await _context.Users
                    .Where(u => u.Id == userId)
                    .Include(x => x.Country)
                    .Select(x => new GetByIdDto
                    {
                        Country = x.Country,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        UserType = (int)x.UserType,
                        PhoneNumber = x.PhoneNumber
                    }).FirstOrDefaultAsync();
        }
    }
}
