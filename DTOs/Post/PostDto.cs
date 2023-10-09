namespace BlogAPIDotnet6.DTOs;

public class PostDto
{
    public string Title { get; set; }
    public string Body { get; set; }
    public bool IsPublished { get; set; }
    public Guid? CategoryId { get; set; }
}