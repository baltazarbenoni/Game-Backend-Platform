namespace Domain.Entities
{
    public class RefreshToken
    {
        public RefreshToken(Guid userId, string token)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Token = token;
            ExpiresAt = DateTime.UtcNow.AddDays(7);
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