namespace BlogAPIDotnet6.Models;

public class UserModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; }
    public string Email { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public UserModel()
    {
        CreatedAt = DateTime.UtcNow;
    }
}