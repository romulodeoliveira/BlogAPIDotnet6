namespace BlogAPIDotnet6.DTOs.User;

public class PublicInfo
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public Guid? AddressId { get; set; }
}