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
    private readonly IUserRepository _userRepository;

    public PostController(IPostRepository postRepository, IUserRepository userRepository)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
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

    [Authorize]
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
            
            post.IsPublished = isPublished;

            var username = User.Identity.Name;

            var user = _userRepository.GetUserByUsername(username);
            if (user != null)
            {
                post.Username = username;
                post.User = user;
                _postRepository.AddPost(post);
                
                user.Posts.Add(post);
                _userRepository.UpdateUser(user);

                return Ok(post);
            }
            else
            {
                return BadRequest("Usuário não encontrado.");
            }
        }
        catch (Exception error)
        {
            return StatusCode(500, $"Não conseguimos publicar sua postagem.\nDetalhe do erro: {error.Message}");
        }
    }

    [Authorize]
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

            post.UpdatedAt = DateTime.UtcNow;

            _postRepository.UpdatePost(post);
            return Ok(post);
        }
        catch (Exception error)
        {
            return StatusCode(500, $"Não conseguimos atualizar sua postagem.\nDetalhe do erro: {error.Message}");
        }
    }
    
    [Authorize]
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