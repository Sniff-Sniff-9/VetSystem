using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using VetSystemApi.Services.Interfaces;
using VetSystemModels.DataBase;
using VetSystemModels.Dto;

namespace VetSystemApi.Services
{
    public class UsersService: IUsersService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<UsersService> _logger;

        private const string DEFAULT_ROLE = "Клиент";
        private static readonly Regex emailRegex = new Regex(@"^[a-zA-Z0-9._+\-%]+@[A-Za-z0-9.-]+\.[a-zA-Z]{2,}$");

        public UsersService(AppDbContext context, IPasswordHasher<User> passwordHasher, ILogger<UsersService> logger)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<List<UserDto>> GetUsersAsync()
        {
            var users = await _context.Users.Include(u => u.Role).Select(u => new UserDto
            {
                Username = u.Username,
                Email = u.Email,
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
                RoleName = user.Role != null ? user.Role.RoleName : "undefiend"
            };
            return userDto;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == DEFAULT_ROLE);

            if (role == null) 
            { 
                throw new InvalidOperationException($"Default role {DEFAULT_ROLE} not found in data base."); 
            }

            if (!emailRegex.IsMatch(createUserDto.Email))
            {
                throw new ArgumentException("Email is incorrect.");
            }

            var user = new User
            {
                Username = createUserDto.Username,
                Email = createUserDto.Email,
                RoleId = role.RoleId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            if (string.IsNullOrWhiteSpace(createUserDto.Password))
            {
                throw new ArgumentException("Password is empty.");
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, createUserDto.Password);

            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return new UserDto
                {
                    Username = user.Username,
                    Email = user.Email,
                    RoleName = role.RoleName
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"User can't be created.");
                throw;
            }
        }

        public async Task<UpdateUserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                throw new ArgumentNullException("User not found.");
            }

            if (!emailRegex.IsMatch(updateUserDto.Email))
            {
                throw new ArgumentException("Email is incorrect.");
            }

            user.Username = updateUserDto.Username;
            user.Email = updateUserDto.Email;

            try
            {
                await _context.SaveChangesAsync();
                return updateUserDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"User can't be updated.");
                throw;
            }
        }

    }
}
