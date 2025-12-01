using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class AuthService : IUserService
    {
        private readonly PasswordHasher<User> hasher = new PasswordHasher<User>();
        public void RegisterAsync(string email, string password, string displayName)
        {

        }
        public void LoginAsync(string email, string password)
        {

        }
        public string HashPassword(User user, string password)
        {
            return hasher.HashPassword(user, password);
        }

        /*
Validate input (length, format, duplicates)

Create new User entity

Save User via repository
        */
    }
}