using System.Security.Cryptography;
using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Application.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;

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
        #region Registration
        public async Task RegisterAsync(string email, string password, string displayName)
        {
            registrationValidator.Validate(email, password, displayName);
            var existingUser = await userRepository.GetByEmailAsync(email);
            if(existingUser != null)
            {
                throw new BadRequestException("Email already in database.");
            }
            User user = new User(email, displayName);
            string hashedPassword = HashPassword(user, password);
            user.SetPassword(hashedPassword);
            await userRepository.AddAsync(user);
        }
        #endregion
        #region Login
        public async Task<AuthResponseDto?> LoginAsync(string email, string password)
        {
            var user = await userRepository.GetByEmailAsync(email);
            if(user == null)
            {
                throw new NotFoundException("Invalid email, couldn't fetch user.");
            }
            PasswordVerificationResult result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if(result == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedException("Password incorrect.");
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
        public async Task<AuthResponseDto?> RefreshTokenAsync(string? refreshToken)
        {
            if(refreshToken == null)
            {
                throw new BadRequestException("Couldn't extract token from request");
            }
            var token = await refreshTokenRepository.GetByTokenAsync(HashRefreshToken(refreshToken));
            if(token == null || token.IsActive == false)
            {
                throw new UnauthorizedException("Invalid refresh token.");
            }
            var user = await userRepository.GetUserByIdAsync(token.UserId);
            if(user == null)
            {
                throw new NotFoundException("User not found for the given refresh token.");
            }
            token.Revoke();
            return await GenerateTokens(user);
        }
        async Task<AuthResponseDto?> GenerateTokens(User user)
        {
            var token = jwtService.GenerateToken(user);
            var refresh = await GenerateRefreshToken(user);
            if(refresh == null)
            {
                throw new BadRequestException("Could not save refresh token.");
            }
            var response =  new AuthResponseDto{ AccessToken = token, RefreshToken = refresh };
            return response;
        }
        async Task<string?> GenerateRefreshToken(User user)
        {
            var token = jwtService.GenerateRefreshToken();
            string hashed = HashRefreshToken(token); 
            var refreshToken = new RefreshToken(user.Id, hashed);
            await refreshTokenRepository.CreateAsync(refreshToken);
            return token;
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
        public async Task RevokeAllRefreshTokensAsync(string? userId)
        {
            if(userId == null)
            {
                throw new BadRequestException("Couln't extract user from request");
            }
            Guid id = new Guid(userId);
            await refreshTokenRepository.RevokeAll(id);
        }
        #endregion
    }
}