using CakeSystem.Domain;

namespace CakeSystem.Application.Services.Auth
{
    public interface IAuthRepository
    {
        Task<bool> AddUser(
            User user, 
            CancellationToken token);
    }
}
