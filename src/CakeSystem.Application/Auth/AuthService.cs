using CakeSystem.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace CakeSystem.Application.Auth
{
    internal class AuthService : IAuthService
    {
        public async Task AddAsync(NewUserDto userDto, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
