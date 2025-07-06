using Application.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> RegisterAsync(RegisterRequest request);
        Task<TokenResponse> LoginAsync(LoginRequest request);
        Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request);
        Task<UserDto> GetProfileAsync(Guid userId);
        Task UpdateNameAsync(Guid userId, UpdateNameRequest request);
        Task VerifyEmailAsync(string email, string token);
    }
}
