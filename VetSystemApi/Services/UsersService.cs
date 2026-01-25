using Microsoft.EntityFrameworkCore;
using VetSystemModels.DataBase;
using VetSystemModels.Dto;
using VetSystemApi.Services.Interfaces;

namespace VetSystemApi.Services
{
    public class UsersService: IUsersService
    {
        public readonly AppDbContext _context;

        public UsersService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserDto>> GetUsersAsync()
        {
            var users = await _context.Users.Include(u => u.Role).Select(u => new UserDto
            {
                Username = u.Username,
                Email = u.Email,
                IsActive = u.IsActive,
                RoleName = u.Role != null ? u.Role.RoleName : "undefiend"
            }
            ).ToListAsync();

            return users;
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null) 
            { 
                return null; 
            }
            var userDto = new UserDto
            {
                Username = user.Username,
                Email = user.Email,
                IsActive = user.IsActive,
                RoleName = user.Role != null ? user.Role.RoleName : "undefiend"
            };
            return userDto;
        }
    }
}
