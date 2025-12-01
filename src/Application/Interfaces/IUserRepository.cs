using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        public void AddAsync(User user)
        {

        }
        public User GetByEmailAsync(string email)
        {

        }
    }
}