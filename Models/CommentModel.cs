namespace BlogAPIDotnet6.Models;

public class CommentModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; }
    public string Body { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public CommentModel()
    {
        CreatedAt = DateTime.UtcNow;
    }
}