using BlogAPIDotnet6.DTOs;
using BlogAPIDotnet6.Helper;
using BlogAPIDotnet6.Models;
using BlogAPIDotnet6.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("list-comments-by-post")]
    public IActionResult ListCommentsByPost(Guid id) // ID da Publicação
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

    [Authorize]
    [HttpPost("create-comment")]
    public IActionResult CreatePost(Guid id, [FromBody] CreateCommentDto request)
    {
        try
        {
            var comment = new CommentModel();
            
            if (!string.IsNullOrEmpty(request.Body))
            {
                comment.Body = request.Body;
            }
            else if (request.Body.Length > 400)
            {
                return BadRequest("O limite máximo é de 400 caracteres.");
            }
            else
            {
                return BadRequest("O corpo do comentário não pode estar em branco.");
            }
            
            var username = User.Identity.Name;
            
            var user = _userRepository.GetUserByUsername(username);
            var post = _postRepository.GetPostById(id);
            if (user != null)
            {
                if (post.IsPublished)
                {
                    comment.Username = username;
                    comment.User = user;
                    comment.PostId = id;
                    comment.Post = post;
                    _commentRepository.AddComment(comment);
                
                    user.Comments.Add(comment);
                    _userRepository.UpdateUser(user);
                
                    post.Comments.Add(comment);
                    _postRepository.UpdatePost(post);

                    return Ok(comment);
                }
                else
                {
                    return BadRequest("Postagem não publicada. Não é possivel comentar.");
                }
            }
            else
            {
                return BadRequest("Usuário não encontrado.");
            }
        }
        catch (Exception error)
        {
            return StatusCode(500, $"Erro interno: {error.Message}");
        }
    }
}