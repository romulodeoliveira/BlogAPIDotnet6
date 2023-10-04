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
        List<CategoryModel> categories = _categoryRepository.GetAllCategories();
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
            
            return Ok(category);
        }
        catch (Exception error)
        {
            return StatusCode(500, $"Erro interno: {error.Message}");
        }
    }

    [Authorize]
    [HttpPut]
    public IActionResult UpdateCategory(Guid categoryId, [FromBody] UpdateCategory request)
    {
        try
        {
            var category = _categoryRepository.GetCategoryById(categoryId);

            if (!string.IsNullOrEmpty(request.Title))
            {
                category.Title = request.Title;
            }

            _categoryRepository.UpdateCategory(category);
            
            return Ok();
        }
        catch (Exception error)
        {
            return StatusCode(500, $"Erro interno: {error.Message}");
        }
    }
}