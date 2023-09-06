using BlogAPIDotnet6.DTOs;
using BlogAPIDotnet6.Models;
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

    [HttpPost("create-post")]
    public IActionResult CreatePost([FromBody] PostDto request)
    {
        try
        {
            var post = new PostModel();
            
            if (!string.IsNullOrEmpty(request.Title))
            {
                post.Title = request.Title;
            }
            else
            {
                return BadRequest("O título da publicação não pode estar em branco.");
            }

            if (!string.IsNullOrEmpty(request.Body))
            {
                post.Body = request.Body;
            }
            else
            {
                return BadRequest("O corpo da publicação não pode estar em branco.");
            }

            _postRepository.AddPost(post);
            return Ok(post);
        }
        catch (Exception error)
        {
            return StatusCode(500, $"Não conseguimos publicar sua postagem.\nDetalhe do erro: {error.Message}");
        }
    }

    [HttpPut("update-post")]
    public IActionResult UpdatePost(Guid id, [FromBody] PostDto request)
    {
        try
        {
            var post = _postRepository.GetPostById(id);
            
            if (!string.IsNullOrEmpty(request.Title))
            {
                post.Title = request.Title;
            }

            if (!string.IsNullOrEmpty(request.Body))
            {
                post.Body = request.Body;
            }

            post.UpdatedAt = DateTime.UtcNow;

            _postRepository.UpdatePost(post);
            return Ok(post);
        }
        catch (Exception error)
        {
            return StatusCode(500, $"Não conseguimos atualizar sua postagem.\nDetalhe do erro: {error.Message}");
        }
    }
}