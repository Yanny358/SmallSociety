namespace Application.Activities;

public class AtendeeDTO
{
    public string DisplayName { get; set; }
    public string Username { get; set; }
    public string? Image { get; set; }
    public bool Following { get; set; }
    public int FollowingCount { get; set; }
    public int FollowersCount { get; set; }
}