namespace BlogAPIDotnet6.DTOs.User;

public class Update
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } = string.Empty;
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
}