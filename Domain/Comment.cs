namespace Domain;

public class Comment
{
    public int Id { get; set; }
    public string Body { get; set; } = default!;
    public AppUser Author { get; set; } = default!;
    public Activity Activity { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}