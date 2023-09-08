using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPIDotnet6.Models;

public class UserModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string Username { get; set; }
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string Role { get; set; }
    public Guid? AddressId { get; set; }
    [ForeignKey(nameof(AddressId))]
    public virtual AddressModel? Address { get; set; }
    public ICollection<PostModel> Posts { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public UserModel()
    {
        CreatedAt = DateTime.UtcNow;
    }
}