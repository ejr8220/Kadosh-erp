namespace Domain.Entities.Security
{
    public class PasswordHistory : AuditoryEntity
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime ChangedAt { get; set; }
    }
}
