using BlogAPIDotnet6.Models;
using BlogAPIDotnet6.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPIDotnet6.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;

    public CategoryController(ICategoryRepository categoryRepository, IPostRepository postRepository,
        IUserRepository userRepository)
    {
        _categoryRepository = categoryRepository;
        _postRepository = postRepository;
        _userRepository = userRepository;
    }
}