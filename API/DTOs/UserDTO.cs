namespace API.DTOs;

public class UserDTO
{
    public string DisplayName { get; set; } = default!;
    public string Token { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string? Image { get; set; }
}