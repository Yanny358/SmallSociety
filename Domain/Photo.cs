namespace Domain;

public class Photo
{
    public string Id { get; set; } = default!;
    public string Url { get; set; } = default!;
    public bool IsMain { get; set; }
}