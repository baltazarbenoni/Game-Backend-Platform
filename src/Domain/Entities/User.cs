using Domain.Exceptions;

namespace Domain.Entities
{
    public class User
    {
        public User(string email, string displayName)
        {
            Email = email;
            DisplayName = displayName;
            PasswordHash = "";
            CreatedAt = DateTime.UtcNow;
            LastLogin = CreatedAt;
        }
        public Guid Id { get; set; } 
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string DisplayName { get; set; }
        public int Level { get; set; }
        public int GamesPlayed { get; set; }
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
        public void IncrementGamesPlayed()
        {
            GamesPlayed++;
        }
        public void SetPassword(string password)
        {
            PasswordHash = password;
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