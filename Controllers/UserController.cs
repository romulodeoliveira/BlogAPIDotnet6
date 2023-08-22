using BlogAPIDotnet6.DTOs;
using BlogAPIDotnet6.Helper;
using BlogAPIDotnet6.Models;
using BlogAPIDotnet6.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPIDotnet6.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public UserController(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDto request)
    {
        var passwordHelper = new PasswordHelper();
        passwordHelper.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var user = new UserModel()
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = Roles.User
        };

        _userRepository.AddUser(user);

        return Ok(user);
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserLoginDto request)
    {
        var user = _userRepository.GetUserByUsername(request.Username);
        
        if (user == null)
        {
            return BadRequest("User not found.");
        }

        var passwordHelper = new PasswordHelper();

        if (!passwordHelper.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            return BadRequest("Wrong password.");
        }

        var tokenHelper = new TokenHelper();
        string token = tokenHelper.CreateToken(user, _configuration);

        return Ok(token);
    }
}