using BlogAPIDotnet6.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPIDotnet6.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;

    public CommentController(ICommentRepository commentRepository, IPostRepository postRepository,
        IUserRepository userRepository)
    {
        _commentRepository = commentRepository;
        _postRepository = postRepository;
        _userRepository = userRepository;
    }

    [HttpGet("list-comments")]
    public IActionResult ListComments(Guid id) // ID da Publicação
    {
        try
        {
            var comments = _commentRepository.GetAllCommentsForPublication(id)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            return Ok(comments);
        }
        catch (Exception error)
        {
            return StatusCode(500, $"Erro interno: {error.Message}");
        }
    }
}