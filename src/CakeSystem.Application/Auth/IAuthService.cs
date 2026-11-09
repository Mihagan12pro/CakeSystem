using CakeSystem.Application.DTOs.Auth;

namespace CakeSystem.Application.Auth
{
    public interface IAuthService
    {
        public Task AddAsync(
            NewUserDto userDto,
            CancellationToken token);
    }
}
