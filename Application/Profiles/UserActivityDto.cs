using System.Text.Json.Serialization;

namespace Application.Profiles;

public class UserActivityDto
{
    public Guid Id { get; set; } 
    public string Title { get; set; } = default!;
    public string Category { get; set; } = default!;
    public DateTime Date { get; set; }
    [JsonIgnore]
    public string HostUsername { get; set; } = default!;
}