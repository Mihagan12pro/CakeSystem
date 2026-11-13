using CakeSystem.Application.DTOs.Auth;

namespace CakeSystem.Application.Services.Auth
{
    public interface IAuthService
    {
        public Task<bool> AddAsync(
            NewUserDto userDto,
            CancellationToken token);

        public Task PutOrAddAsync(
            PutUserDto userDto,
            CancellationToken token);

        public Task<GetUserDto> GetUser(
            LoginUserDto userDto,
            CancellationToken token);
    }
}
