using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BlogAPIDotnet6.Models;

public class PostModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; }
    public string Body { get; set; }
    public string Username { get; set; }
    public bool IsPublished { get; set; }
    [Index(IsUnique = true)]
    public string Slug { get; set; }
    [ForeignKey(nameof(Username))]
    public virtual UserModel User { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public PostModel()
    {
        CreatedAt = DateTime.UtcNow;
    }
}