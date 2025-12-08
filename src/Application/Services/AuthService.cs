using System.Security.Cryptography;
using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class AuthService : IUserService
    {
        public AuthService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, RegistrationValidator validator, JwtService jwtService)
        {
            this.userRepository = userRepository;
            this.refreshTokenRepository = refreshTokenRepository;
            this.registrationValidator = validator;
            this.jwtService = jwtService;
        }
        private readonly JwtService jwtService;
        private readonly IUserRepository userRepository;
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly RegistrationValidator registrationValidator;
        private readonly PasswordHasher<User> hasher = new();
        public async Task RegisterAsync(string email, string password, string displayName)
        {
            registrationValidator.Validate(email, password, displayName);
            var existingUser = await userRepository.GetByEmailAsync(email);
            if(existingUser != null)
            {
                throw new Exception("Email already in database.");
            }
            User user = new User(email, displayName);
            string hashedPassword = HashPassword(user, password);
            user.SetPassword(hashedPassword);
            await userRepository.AddAsync(user);
        }
        #region Login
        public async Task<AuthResponseDto?> LoginAsync(string email, string password)
        {
            var user = await userRepository.GetByEmailAsync(email);
            if(user == null)
            {
                throw new Exception("Invalid email, couldn't fetch user.");
            }
            PasswordVerificationResult result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if(result == PasswordVerificationResult.Failed)
            {
                throw new Exception("Password incorrect.");
            }
            if(result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                string newPassword = HashPassword(user, password);
                user.SetPassword(newPassword);
                await userRepository.UpdateAsync(user);
            }
            return await GenerateTokens(user);
        }
        public string HashPassword(User user, string password)
        {
            return hasher.HashPassword(user, password);
        }
        #endregion
        #region Refresh Token
        public async Task<AuthResponseDto?> RefreshTokenAsync(string refreshToken)
        {
            var token = refreshTokenRepository.GetByTokenAsync(HashRefreshToken(refreshToken));
            if(token == null)
            {
                throw new Exception("Invalid refresh token.");
            }
            var user = await userRepository.GetUserByIdAsync(token.Result!.UserId);
            if(user == null)
            {
                throw new Exception("User not found for the given refresh token.");
            }
            return await GenerateTokens(user);
        }
        async Task<AuthResponseDto?> GenerateTokens(User user)
        {
            var token = jwtService.GenerateToken(user);
            var refresh = await GenerateRefreshToken(user);
            if(refresh == null)
            {
                throw new Exception("Could not save refresh token.");
            }
            var response =  new AuthResponseDto{ AccessToken = token, RefreshToken = refresh };
            return response;
        }

        public async Task<string?> GenerateRefreshToken(User user)
        {
            var token = jwtService.GenerateRefreshToken();
            string hashed = HashRefreshToken(token); 
            var refreshToken = new RefreshToken(user.Id, hashed);
            await refreshTokenRepository.AddAsync(refreshToken);
            return refreshToken.Token;
        }
        string HashRefreshToken(string refreshToken)
        {
            using(var sha256 = SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(refreshToken);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        #endregion
    }
}