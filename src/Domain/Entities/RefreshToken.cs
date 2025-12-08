namespace Domain.Entities
{
    public class RefreshToken
    {
        public RefreshToken(string userId, string token, DateTime expiresAt)
        {
            Id = Guid.NewGuid();
            UserId = new Guid(userId);
            Token = token;
            ExpiresAt = expiresAt;
            CreatedAt = DateTime.UtcNow;
        }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RevokedAt { get; set; }

        public bool IsActive => RevokedAt == null && DateTime.UtcNow < ExpiresAt;
        public string? ReplacedByTokenHash { get; set; } 
        public void Revoke(string? replacedByTokenHash = null)
        {
            RevokedAt = DateTime.UtcNow;
            ReplacedByTokenHash = replacedByTokenHash;
        }
    }
}