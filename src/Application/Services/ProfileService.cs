using Application.DTOs;
using Application.Interfaces;

namespace Application.Services
{
    public class ProfileService
    {
        public ProfileService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        private readonly IUserRepository userRepository;

        public async Task<ProfileRequest?> GetProfileAsync(string id)
        {
            var user = await userRepository.GetUserByIdAsync(new Guid(id));
            if(user == null)
            {
                throw new Exception("User not found.");
            }
            var profile = new ProfileRequest{ DisplayName = user.DisplayName, Email = user.Email, Message = $"Welcome to your profile {user.DisplayName}!"};
            return profile; 
        }
        public async Task<StatsDto?> GetStatsAsync(string id)
        {
            var user = await userRepository.GetUserByIdAsync(new Guid(id));
            if(user == null)
            {
                throw new Exception("User not found.");
            }
            var stats = new StatsDto{ Level = user.Level, GamesPlayed = user.GamesPlayed, CreatedAt = user.CreatedAt};
            return stats; 
        }
        public async Task UpdateDisplayNameAsync(string id, string newDisplayName)
        {
            var user = await userRepository.GetUserByIdAsync(new Guid(id));
            if(user == null)
            {
                throw new Exception("User not found.");
            }
            user.ChangeName(newDisplayName);
            await userRepository.UpdateAsync(user);
        }
    }
}