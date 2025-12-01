namespace Application.Interfaces
{
    public interface IUserService
    {
        public void RegisterAsync(string email, string password, string displayName);
        public void LoginAsync(string email, string password);
    }
}