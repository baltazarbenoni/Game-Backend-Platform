using Application.Interfaces;
using Application.Validators;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class AuthService : IUserService
    {
        public AuthService(IUserRepository userRepository, RegistrationValidator validator, JwtService jwtService)
        {
            this.userRepository = userRepository;
            this.registrationValidator = validator;
            this.jwtService = jwtService;
        }
        private readonly JwtService jwtService;
        private readonly IUserRepository userRepository;
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
        public async Task<string?> LoginAsync(string email, string password)
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
            return jwtService.GenerateToken(user);
        }
        public string HashPassword(User user, string password)
        {
            return hasher.HashPassword(user, password);
        }
    }
}