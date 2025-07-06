using Application.DTOS;
using Application.Interfaces;
using Domain.Entities;
using Domain.Helper;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        public UserService(IUserRepository userRepository,IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }
        public async Task<UserDto> GetProfileAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new AuthException("User not found.");

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name
            };
        }

        public async Task<TokenResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || user.Password != request.Password)
            {
                throw new AuthException("Invalid email or password.");
            }
            var accessToken = _jwtService.GenerateToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateAsync(user);
            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

        }

        public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                throw new AuthException("Invalid or expired refresh token.");
            }
            var newAccessToken = _jwtService.GenerateToken(user);
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateAsync(user);

            return new TokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = user.RefreshToken
            };
        }

        public async Task<UserDto> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
                throw new AuthException("Email already registered.");
            var user = new User(request.Email, request.Password, request.Name);
            user.GenerateEmailVerificationToken();
            await _userRepository.AddAsync(user);
            Console.WriteLine($"[Mock Email Sent] Token: {user.EmailVerificationToken}");
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name
            };
        }

        public async Task UpdateNameAsync(Guid userId, UpdateNameRequest request)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new AuthException("User not found.");

            user.UpdateName(request.Name);
            await _userRepository.UpdateAsync(user);
        }

        public async Task VerifyEmailAsync(string email, string token)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                throw new AuthException("User not found.");

            user.VerifyEmail(token);
            await _userRepository.UpdateAsync(user);
        }

    }
}
