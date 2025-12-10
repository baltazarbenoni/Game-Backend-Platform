using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Application.Services
{
    public class ProfileService
    {
        public ProfileService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        private readonly IUserRepository userRepository;

        public async Task<ProfileRequest?> GetProfileAsync(string? id)
        {
            if(id == null)
            {
                throw new BadRequestException("Couldn't extract user from request");
            }
            var guid = new Guid(id);
            var user = await userRepository.GetUserByIdAsync(guid);
            if(user == null)
            {
                throw new NotFoundException("User not found.");
            }
            var profile = new ProfileRequest{ DisplayName = user.DisplayName, Email = user.Email, Message = $"Welcome to your profile {user.DisplayName}!"};
            return profile; 
        }
        public async Task<StatsDto?> GetStatsAsync(string? id)
        {
            if(id == null)
            {
                throw new BadRequestException("Couldn't extract user from request");
            }
            var user = await userRepository.GetUserByIdAsync(new Guid(id));
            if(user == null)
            {
                throw new NotFoundException("User not found.");
            }
            var stats = new StatsDto{ Level = user.Level, GamesPlayed = user.GamesPlayed, CreatedAt = user.CreatedAt};
            return stats; 
        }
        public async Task UpdateDisplayNameAsync(string id, string newDisplayName)
        {
            var user = await userRepository.GetUserByIdAsync(new Guid(id));
            if(user == null)
            {
                throw new NotFoundException("User not found.");
            }
            user.ChangeName(newDisplayName);
            await userRepository.UpdateAsync(user);
        }
    }
}