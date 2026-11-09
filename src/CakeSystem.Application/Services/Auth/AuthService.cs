using CakeSystem.Application.DTOs.Auth;
using CakeSystem.Domain;
using CakeSystem.Domain.Enums;

namespace CakeSystem.Application.Services.Auth
{
    internal class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public async Task AddAsync(NewUserDto userDto, CancellationToken token)
        {
            User user = new User
            {
                Login = userDto.Login,

                Password = userDto.Password,

                Role = Roles.Client
            };
        }

        public async Task AddAsync(NewUserDto userDto, string roleStr, CancellationToken token)
        {
            Roles role;

            switch(roleStr)
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
                        role = Roles.AccountManaher;
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

                Password = userDto.Password,

                Role = role
            };
        }

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
    }
}
