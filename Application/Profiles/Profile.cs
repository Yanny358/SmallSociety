using Domain;

namespace Application.Profiles;

public class Profile
{
    public string DisplayName { get; set; }  = default!;
    public string Username { get; set; }  = default!;
    public string? Image { get; set; }
    public bool Following { get; set; }
    public int FollowersCount { get; set; }
    public int FollowingCount { get; set; }
    public ICollection<Photo>? Photos { get; set; }
}