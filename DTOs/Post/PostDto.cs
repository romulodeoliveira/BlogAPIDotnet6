namespace BlogAPIDotnet6.DTOs;

public class PostDto
{
    private string Title { get; set; }
    private string Body { get; set; }
    private Guid? CategoryId { get; set; }
}