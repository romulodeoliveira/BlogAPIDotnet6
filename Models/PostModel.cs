using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BlogAPIDotnet6.Models;

public class PostModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; }
    public string Body { get; set; }
    public bool IsPublished { get; set; }
    public string Slug { get; set; }
    public string Username { get; set; }
    [ForeignKey(nameof(Username))]
    public virtual UserModel User { get; set; }
    public ICollection<CommentModel>? Comments { get; set; }
    public Guid? CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))]
    public virtual CategoryModel? Category { get; set; } 
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public PostModel()
    {
        CreatedAt = DateTime.UtcNow;
    }
}