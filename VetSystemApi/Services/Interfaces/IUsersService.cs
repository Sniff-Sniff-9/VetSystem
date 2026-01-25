using VetSystemModels.Dto;

namespace VetSystemApi.Services.Interfaces
{
    public interface IUsersService
    {
        public Task<List<UserDto>> GetUsersAsync();
        public Task<UserDto?> GetUserByIdAsync(int id);
        public Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
        public Task<UpdateUserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
        //public Task DeleteUserAsync(int id);
    }
}
