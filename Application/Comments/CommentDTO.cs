namespace Application.Comments;

public class CommentDTO
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Body { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public string? Image { get; set; }
}