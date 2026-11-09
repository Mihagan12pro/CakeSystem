using CakeSystem.Application.Services.Auth;
using CakeSystem.Domain;

namespace CakeSystem.Infrastructure.SQLite.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        public Task<bool> AddUser(User user, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
