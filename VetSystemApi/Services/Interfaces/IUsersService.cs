using VetSystemModels.Dto.User;

namespace VetSystemApi.Services.Interfaces
{
    public interface IUsersService
    {
        public Task<List<UserDto>> GetUsersAsync();
        public Task<UserDto?> GetUserByIdAsync(int id);
        public Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
        public Task<UserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
        public Task DeleteUserAsync(int id);
    }
}
