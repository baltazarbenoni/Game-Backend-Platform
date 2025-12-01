using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task RegisterAsync(string email, string password, string displayName);
        Task<User?> LoginAsync(string email, string password);
    }
}