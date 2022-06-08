using Microsoft.AspNetCore.Identity;

namespace Domain;

public class AppUser : IdentityUser
{
    public string DisplayName { get; set; } = default!;
    public ICollection<ActivityAtendee>? Activities { get; set; }
    public ICollection<Photo>? Photos { get; set; }
    public ICollection<UserFollowing>? Followings { get; set; }
    public ICollection<UserFollowing>? Followers { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

}