namespace Application.Interfaces
{
    public interface IUserService
    {
        Task RegisterAsync(string email, string password, string displayName);
        Task LoginAsync(string email, string password);
    }
}