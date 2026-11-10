using CakeSystem.Domain;

namespace CakeSystem.Application.Services.Auth
{
    public interface IAuthRepository
    {
        Task<bool> AddUserAsync(
            User user, 
            CancellationToken token);

        Task PutOrAddUserAsync(
            User user,
            CancellationToken token);
    }
}
