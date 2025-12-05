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

        public async Task<string?> GetProfileAsync(string id)
        {
            var user = await userRepository.GetByEmailAsync(id);
            if(user == null)
            {
                throw new Exception("User not found.");
            }
            return user.DisplayName;
        }
    }
}