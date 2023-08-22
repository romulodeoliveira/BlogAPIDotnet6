namespace BlogAPIDotnet6.DTOs;

public class UserProfileUpdateDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } = string.Empty;
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
}