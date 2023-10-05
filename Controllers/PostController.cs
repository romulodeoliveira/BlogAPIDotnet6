using BlogAPIDotnet6.DTOs;
using BlogAPIDotnet6.Helper;
using BlogAPIDotnet6.Models;
using BlogAPIDotnet6.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
                .Select(post => new
                {
                    post.Slug,
                    post.Title,
                    post.Username,
                    post.IsPublished,
                    post.Comments,
                    post.CreatedAt,
                    post.UpdatedAt
                })
                .ToList();

            return Ok(posts);
        }
        catch (Exception error)
        {
            return StatusCode(500, $"Erro interno do servidor: {error.Message}");
        }
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPost("create-post")]
    public IActionResult CreatePost(bool isPublished, [FromBody] PostDto request)
    {
        try
        {
            var post = new PostModel();
            
            if (!string.IsNullOrEmpty(request.Title))
            {
                post.Title = request.Title;

                string slug = SlugHelper.GenerateSlug(request.Title);
                
                if (!_postRepository.IsSlugUnique(slug))
                {
                    return BadRequest("Slug já existe. Escolha um título diferente.");
                }
                else
                {
                    post.Slug = slug;
                }
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

            if (request.CategoryId != null)
            {
                post.CategoryId = request.CategoryId;
            }
            
            post.IsPublished = isPublished;

            var username = User.Identity.Name;
            post.Username = username;
            
            _postRepository.AddPost(post);

            var info = new
            {
                post.Slug,
                post.Title,
                post.Username,
                post.IsPublished,
                post.Comments,
                post.CreatedAt,
                post.UpdatedAt
            };
            return Ok(info);
        }
        catch (Exception error)
        {
            return StatusCode(500, $"Não conseguimos publicar sua postagem.\nDetalhe do erro: {error.Message}");
        }
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPut("update-post")]
    public IActionResult UpdatePost(Guid id, [FromBody] PostDto request)
    {
        try
        {
            var post = _postRepository.GetPostById(id);
            
            if (!string.IsNullOrEmpty(request.Title))
            {
                post.Title = request.Title;
                
                string slug = SlugHelper.GenerateSlug(request.Title);
                
                if (!_postRepository.IsSlugUnique(slug))
                {
                    return BadRequest("Slug já existe. Escolha um título diferente.");
                }
                else
                {
                    post.Slug = slug;
                }
            }

            if (!string.IsNullOrEmpty(request.Body))
            {
                post.Body = request.Body;
            }
            
            if (request.CategoryId != null)
            {
                post.CategoryId = request.CategoryId;
            }

            post.UpdatedAt = DateTime.UtcNow;

            _postRepository.UpdatePost(post);
            
            var info = new
            {
                post.Slug,
                post.Title,
                post.Username,
                post.IsPublished,
                post.Comments,
                post.CreatedAt,
                post.UpdatedAt
            };
            return Ok(info);
        }
        catch (Exception error)
        {
            return StatusCode(500, $"Não conseguimos atualizar sua postagem.\nDetalhe do erro: {error.Message}");
        }
    }
    
    [Authorize(Roles = Roles.Admin)]
    [HttpDelete("delete-post")]
    public IActionResult DeletePost(Guid id)
    {
        try
        {
            bool deleted = _postRepository.DeletePost(id);
                
            if (deleted)
            {
                return Ok("Publicação deletada com sucesso.");
            }
            else
            {
                return NotFound("Publicação não encontrado.");
            }
        }
        catch (Exception error)
        {
            return StatusCode(500, $"Não conseguimos apagar sua postagem.\nDetalhe do erro: {error.Message}");
        }
    }
}