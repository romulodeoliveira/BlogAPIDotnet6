using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPIDotnet6.Models;

public class CommentModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Body { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string Username { get; set; }
    [ForeignKey(nameof(Username))]
    public virtual UserModel User { get; set; }
    public Guid PostId { get; set; }
    [ForeignKey(nameof(PostId))]
    public virtual PostModel Post { get; set; }

    public CommentModel()
    {
        CreatedAt = DateTime.UtcNow;
    }
}