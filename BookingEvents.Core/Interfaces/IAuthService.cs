using BookingEvents.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingEvents.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthDto> LoginAsync(LoginDto loginDto);
        Task<string> AddRoleAsync(AddRoleDto addRoleDto);
        Task<AuthDto> RefreshTokenAsync(string refreshToken);
        Task<bool> RevokeTokenAsync(string refreshToken);
    }
}
