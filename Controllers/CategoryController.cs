using BlogAPIDotnet6.DTOs.Category;
using BlogAPIDotnet6.Models;
using BlogAPIDotnet6.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPIDotnet6.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpGet("list-categories")]
    public IActionResult ListCategories()
    {
        var categories = _categoryRepository.GetAllCategories()
            .Select(c => new
            {
                c.Id,
                c.Title,
                c.Username,
                c.Posts,
                c.CreatedAt,
                c.UpdatedAt
            });
        return Ok(categories);
    }
    
    [Authorize]
    [HttpPost("create-category")]
    public IActionResult CreateCategory([FromBody] CreateCategory request)
    {
        try
        {
            var category = new CategoryModel();

            if (!string.IsNullOrEmpty(request.Title))
            {
                category.Title = request.Title;
            }
            else
            {
                return BadRequest("O título da categoria não pode estar em branco.");
            }

            var username = User.Identity.Name;
            category.Username = username;

            _categoryRepository.AddCategory(category);

            var info = new
            {
                category.Id,
                category.Title,
                category.Username,
                category.Posts,
                category.CreatedAt,
                category.UpdatedAt
            };
            
            return Ok(info);
        }
        catch (Exception error)
        {
            return StatusCode(500, $"Erro interno: {error.Message}");
        }
    }

    [Authorize]
    [HttpPut("update-category/{categoryId}")]
    public IActionResult UpdateCategory([FromRoute] Guid categoryId, [FromBody] UpdateCategory request)
    {
        try
        {
            var category = _categoryRepository.GetCategoryById(categoryId);

            if (!string.IsNullOrEmpty(request.Title))
            {
                category.Title = request.Title;
            }

            _categoryRepository.UpdateCategory(category);
            
            var info = new
            {
                category.Id,
                category.Title,
                category.Username,
                category.Posts,
                category.CreatedAt,
                category.UpdatedAt
            };

            return Ok(info);
        }
        catch (Exception error)
        {
            return StatusCode(500, $"Erro interno: {error.Message}");
        }
    }
    
    [Authorize]
    [HttpDelete("delete-category/{categoryId}")]
    public IActionResult DeleteCategory([FromRoute] Guid categoryId)
    {
        try
        {
            bool deleted = _categoryRepository.DeleteCategory(categoryId);

            if (deleted)
            {
                return Ok("Categoria deletada com sucesso.");
            }
            else
            {
                return NotFound("Categoria não encontrada.");
            }
        }
        catch (Exception error)
        {
            return StatusCode(500, $"Erro interno: {error.Message}");
        }
    }
}