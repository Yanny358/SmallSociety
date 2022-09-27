namespace Domain;

public class ActivityAttendee
{
    public string AppUserId { get; set; } = default!;
    public AppUser AppUser { get; set; } = default!;
    public Guid ActivityId { get; set; } = default!;
    public Activity Activity { get; set; } = default!;
    public bool IsHost { get; set; }
}