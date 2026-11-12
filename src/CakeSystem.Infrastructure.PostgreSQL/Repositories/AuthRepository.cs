using CakeSystem.Application.Services.Auth;
using CakeSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace CakeSystem.Infrastructure.PostgreSQL.Repositories
{
    internal class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _dbContext;

        public async Task<bool> AddUserAsync(User user, CancellationToken token)
        {
            if (await _dbContext.Users.FirstOrDefaultAsync(u => u.Login == user.Login) != null)
                return false;

            await _dbContext.Users.AddAsync(user, token);
            await _dbContext.SaveChangesAsync(token);

            return true;
        }

        public async Task PutOrAddUserAsync(User user, CancellationToken token)
        {
            var hostedUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Login == user.Login);

            if (hostedUser == null)
            {
                await AddUserAsync(user, token);

                return;
            }

            hostedUser.Password = user.Password;
            hostedUser.FullName = user.FullName;
            hostedUser.Role = user.Role;

            await _dbContext.SaveChangesAsync(token);
        }

        public AuthRepository(AppDbContext dbContext)
            => _dbContext = dbContext;
    }
}
