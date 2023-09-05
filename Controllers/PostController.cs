using BlogAPIDotnet6.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPIDotnet6.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly IPostRepository _postRepository;

    public PostController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    [HttpGet("list-posts")]
    public IActionResult ListPosts()
    {
        try
        {
            var posts = _postRepository.GetAllPosts()
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            return Ok(posts);
        }
        catch (Exception error)
        {
            return StatusCode(500, $"Erro interno do servidor: {error.Message}");
        }
    }
}