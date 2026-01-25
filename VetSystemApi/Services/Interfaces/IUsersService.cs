using VetSystemModels.DataBase;
using VetSystemModels.Dto;

namespace VetSystemApi.Services.Interfaces
{
    public interface IUsersService
    {
        public Task<List<UserDto>> GetUsersAsync();

        public Task<UserDto?> GetUserByIdAsync(int id);
        //public Task CreateUserAsync(CreateUserDto createUserDto);
        //public Task UpdateUserAsync(int id, UserDto userDto);
        //public Task DeleteUserAsync(int id);
    }
}
