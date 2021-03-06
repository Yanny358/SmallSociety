namespace Domain;

public class RefreshToken
{
    public int Id { get; set; }
    public AppUser AppUser { get; set; } = default!;
    public string Token { get; set; } = default!;
    public DateTime Expire { get; set; } = DateTime.UtcNow.AddDays(7);
    public bool IsExpired => DateTime.UtcNow >= Expire;
    public DateTime? Revoked { get; set; }
    public bool IsActive => Revoked == null && !IsExpired;
}