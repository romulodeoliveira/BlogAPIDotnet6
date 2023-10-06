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

    public CommentController(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    [HttpGet("list-comments-by-post")]
    public IActionResult ListCommentsByPost(Guid postId) // ID da Publicação
    {
        try
        {
            var comments = _commentRepository.GetAllCommentsForPublication(postId)
                .OrderByDescending(p => p.CreatedAt)
                .Select(c => new
                {
                    c.Id,
                    c.Body,
                    c.Username,
                    c.PostId,
                    c.CreatedAt,
                    c.UpdatedAt
                })
                .ToList();

            return Ok(comments);
        }
        catch (Exception error)
        {
            return StatusCode(500, $"Erro interno: {error.Message}");
        }
    }

    [Authorize]
    [HttpGet("list-comments-by-user")]
    public IActionResult ListCommentsByUser()
    {
        try
        {
            var user = User.Identity.Name;

            var comments = _commentRepository.GetAllCommentsForUser(user)
                .OrderByDescending(p => p.CreatedAt)
                .Select(c => new
                {
                    c.Id,
                    c.Body,
                    c.Username,
                    c.PostId,
                    c.CreatedAt,
                    c.UpdatedAt
                })
                .ToList();

            if (user != null)
            {
                return Ok(comments);
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

    [Authorize]
    [HttpPost("create-comment/{postId}")]
    public IActionResult CreatePost([FromRoute] Guid postId, [FromBody] CreateCommentDto request)
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
            
            comment.Username = username;
            comment.PostId = postId;
            _commentRepository.AddComment(comment);

            var info = new
            {
                comment.Id,
                comment.Body,
                comment.Username,
                comment.PostId,
                comment.CreatedAt,
                comment.UpdatedAt
            };
                
            return Ok(info);
        }
        catch (Exception error)
        {
            return StatusCode(500, $"Erro interno: {error.Message}");
        }
    }

    [Authorize]
    [HttpPut("update-comment/{postId}/{commentId}")]
    public IActionResult UpdateComment([FromRoute] Guid postId, [FromRoute] Guid commentId, [FromBody] UpdateCommentDto request)
    {
        try
        {
            var user = User.Identity.Name;
            var comment = _commentRepository.GetCommentById(commentId);

            if (comment == null)
            {
                return NotFound("Comentário não encontrado.");
            }

            if (postId == null)
            {
                return NotFound("Post não encontrado.");
            }
            
            if (user != comment.Username)
            {
                return Unauthorized("Você não tem permissão para atualizar este comentário.");
            }
        
            if (!string.IsNullOrEmpty(request.Body))
            {
                comment.Body = request.Body;
            }
            
            comment.UpdatedAt = DateTime.UtcNow;

            _commentRepository.UpdateComment(comment);
            
            var info = new
            {
                comment.Id,
                comment.Body,
                comment.Username,
                comment.PostId,
                comment.CreatedAt,
                comment.UpdatedAt
            };
                
            return Ok(info);
        }
        catch (Exception error)
        {
            return StatusCode(500, $"Erro interno: {error.Message}");
        }
    }

    [Authorize]
    [HttpDelete("delete-comment/{postId}/{commentId}")]
    public IActionResult DeleteComment([FromRoute] Guid postId, [FromRoute] Guid commentId)
    {
        try
        {
            var user = User.Identity.Name;
            var comment = _commentRepository.GetCommentById(commentId);

            if (comment == null)
            {
                return NotFound("Comentário não encontrado.");
            }

            if (postId == null)
            {
                return NotFound("Post não encontrado.");
            }
            
            if (user != comment.Username)
            {
                return Unauthorized("Você não tem permissão para deletar este comentário.");
            }
            
            bool deleted = _commentRepository.DeleteComment(commentId);
            
            return Ok("Comentário deletada com sucesso.");
        }
        catch (Exception error)
        {
            return StatusCode(500, $"Erro interno: {error.Message}");
        }
    }
}