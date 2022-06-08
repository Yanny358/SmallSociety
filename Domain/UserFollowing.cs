namespace Domain;

public class UserFollowing
{
    public string ObserverId { get; set; } = default!;
    public AppUser Observer { get; set; } = default!;
    public string TargetId { get; set; } = default!;
    public AppUser Target { get; set; } = default!;
} 