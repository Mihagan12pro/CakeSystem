using CakeSystem.Application.Services.Auth;
using CakeSystem.Domain;
using Dapper;
using MySqlConnector;

namespace CakeSystem.Infrastructure.MySQL.Repositories
{
    internal class AuthRepository : IAuthRepository
    {
        private readonly string _connectionString;

        public async Task<bool> AddUserAsync(User user, CancellationToken token)
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync(token);

            var existsCmd = new CommandDefinition(
                "SELECT EXISTS(SELECT 1 FROM Users WHERE Login = @login)",
                new { login = user.Login },
                cancellationToken: token);

            bool exists = await connection.ExecuteScalarAsync<bool>(existsCmd);
            if (exists)
                return false;   

            const string insertSql =
                "INSERT INTO Users (Login, Password, Role, Fullname) " +
                "VALUES (@Login, @Password, @Role, @Fullname)";

            var insertCmd = new CommandDefinition(
                insertSql,
                user,
                cancellationToken: token);

            await connection.ExecuteAsync(insertCmd);

            return true;
        }

        public async Task PutOrAddUserAsync(User user, CancellationToken token)
        {
            if (!await AddUserAsync(user, token))
            {
                await using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync(token);

                const string updateSql = "UPDATE Users " +
                    "SET Password = @Password, Role = @Role, Fullname = @Fullname " +
                    "WHERE Login = @Login";

                var updateCmd = new CommandDefinition(
                    updateSql,
                    user,
                    cancellationToken: token);

                await connection.ExecuteAsync(updateCmd);
            }
        }

        public AuthRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
    }
}
