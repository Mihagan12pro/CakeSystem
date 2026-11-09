using CakeSystem.Application.DTOs.Auth;

namespace CakeSystem.Application.Services.Auth
{
    public interface IAuthService
    {
        public Task AddAsync(
            NewUserDto userDto,
            CancellationToken token);

        public Task AddAsync(
            NewUserDto userDto,
            string Role,
            CancellationToken token);
    }
}
