using CakeSystem.Application.Services.Auth;
using CakeSystem.Domain;

namespace CakeSystem.Infrastructure.SQLite.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _dbContext;

        public async Task<bool> AddUser(User user, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task PutOrAddUser(User user, CancellationToken token)
        {
            
        }

        public AuthRepository(AppDbContext dbContext)
            => _dbContext = dbContext;
    }
}
