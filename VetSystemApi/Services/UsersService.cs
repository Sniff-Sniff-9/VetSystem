using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.RegularExpressions;
using VetSystemInfrastructure.Configuration;
using VetSystemApi.Services.Interfaces;
using VetSystemModels.Entities;
using VetSystemModels.Dto;

namespace VetSystemApi.Services
{
    public class UsersService: IUsersService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<UsersService> _logger;
        private readonly DefaultUserSettings _defaultUserSettings;

        private static readonly Regex emailRegex = new Regex(@"^[a-zA-Z0-9._+\-%]+@[A-Za-z0-9.-]+\.[a-zA-Z]{2,}$");

        public UsersService(AppDbContext context, IPasswordHasher<User> passwordHasher, ILogger<UsersService> logger, IOptions<DefaultUserSettings> defaultUserOptions)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _logger = logger;
            _defaultUserSettings = defaultUserOptions.Value;
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
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == _defaultUserSettings.DefaultRole);

            if (role == null) 
            { 
                throw new InvalidOperationException($"Default role {_defaultUserSettings.DefaultRole} not found in data base."); 
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
                throw new ArgumentNullException("Password is empty.");
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

        public async Task<UserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
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
                return new UserDto
                {
                    Username = user.Username,
                    Email = user.Email,
                    RoleName = user.Role?.RoleName ?? "undefined"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"User can't be updated.");
                throw;
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
            {
                throw new ArgumentNullException("User not found.");
            }
            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "User can't be deleted.");
                throw;
            }

        }
    }
}
