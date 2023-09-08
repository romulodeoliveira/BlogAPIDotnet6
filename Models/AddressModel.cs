using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPIDotnet6.Models;

public class AddressModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string Username { get; set; }
    [ForeignKey(nameof(Username))]
    public virtual UserModel User { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public AddressModel()
    {
        CreatedAt = DateTime.UtcNow;
    }
}