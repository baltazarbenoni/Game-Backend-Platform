public class User
{
    public Guid Id { get; set; } 
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required string Name { get; set; }
    public int Level { get; set; }
    public bool isBanned { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastLogin { get; set; }
    public void LevelUp()
    {
        Level++;
    }
    public void ChangeName(string newName)
    {
        if(newName == "" )
        {
        }
        else
        {
            Name = newName;
        }
    }

}