namespace Domain.Entities.Security
{
    public class PasswordResetToken : AuditoryEntity
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime? UsedAt { get; set; }
        public bool IsUsed { get; set; }
    }
}
