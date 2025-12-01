using Domain.Exceptions;

namespace Domain.Entities
{
    public class User
    {
        public User(string email, string password, string displayName)
        {
            CreatedAt = DateTime.Now;
        }
        public Guid Id { get; set; } 
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required string DisplayName { get; set; }
        public int Level { get; set; }
        public bool isBanned { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastLogin { get; set; }
        public void UpdateLastLogin()
        {
            LastLogin = DateTime.Now;
        }
        public void LevelUp()
        {
            Level++;
        }
        public void ChangeName(string newName)
        {
            if(string.IsNullOrWhiteSpace(newName) || newName.Length < 3)
            {
                throw new InvalidNameException(newName);
            }
            else
            {
                DisplayName = newName;
            }
        }
    }
}