using CakeSystem.Application.DTOs.Auth;
using CakeSystem.Application.Security.Hashers.SHA256;
using CakeSystem.Domain;
using CakeSystem.Domain.Enums;

namespace CakeSystem.Application.Services.Auth
{
    internal class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly ISha256Hasher _hasher;

        public async Task<bool> AddAsync(NewUserDto userDto, CancellationToken token)
        {
            User user = new User
            {
                Login = userDto.Login,

                Password = _hasher.Hash(userDto.Password),

                Role = Roles.Client
            };

            return await _authRepository.AddUserAsync(user, token);
        }

        public async Task PutOrAddAsync(PutUserDto userDto, CancellationToken token)
        {
            Roles role;

            switch(userDto.Role)
            {
                case "Директор":
                    {
                        role = Roles.Manager;
                        break;
                    }

                case "Менеджерпозакупкам":
                    {
                        role = Roles.PurchasingManager;
                        break;
                    }
                case "Менеджерпопродажам":
                    {
                        role = Roles.AccountManager;
                        break;
                    }
                case "Мастер":
                    {
                        role = Roles.Baker;
                        break;
                    }
                case "Заказчик":
                    {
                        role = Roles.Client;
                        break;
                    }
                default:
                    {
                        role = Roles.Client;
                        break;
                    }
            }

            User user = new User
            {
                Login = userDto.Login,

                Password = _hasher.Hash(userDto.Password),

                FullName = userDto.FullName,

                Role = role
            };

            await _authRepository.PutOrAddUserAsync(user, token);
        }

        public async Task<GetUserDto> GetUser(LoginUserDto userDto, CancellationToken token)
        {
            userDto = userDto with { Password = _hasher.Hash(userDto.Password) };

            var user = await _authRepository.GetUserAsync(userDto, token);

            if (user == null)
                return null;

            return new GetUserDto(user.Login, user.FullName, user.Role);
        }

        public AuthService(
            IAuthRepository authRepository,
            ISha256Hasher hasher)
        {
            _authRepository = authRepository;
            _hasher = hasher;
        }
    }
}
