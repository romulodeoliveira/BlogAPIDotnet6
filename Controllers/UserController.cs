using BlogAPIDotnet6.DTOs;
using BlogAPIDotnet6.Helper;
using BlogAPIDotnet6.Models;
using BlogAPIDotnet6.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpGet("owninfo")]
    public IActionResult GetOwnUserInformation()
    {
        try
        {
            var username = User.Identity.Name;
            
            var user = _userRepository.GetUserByUsername(username);
            
            if (user == null)
            {
                return NotFound("Usuário não encontrado.");
            }
            
            return Ok(user);
        }
        catch (System.Exception error)
        {
            return StatusCode(500, $"Ops... Não conseguimos coletar todas as informações do seu perfil. Tente novamente!\nDetalhe do erro: {error.Message}");
        }
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
    
    [Authorize]
    [HttpPut("update-profile")]
    public IActionResult UpdateProfile([FromBody] UserProfileUpdateDto request)
    {
        try
        {
            var username = User.Identity.Name;
            var user = _userRepository.GetUserByUsername(username);
            
            if (user == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            if (user.Username != request.Username)
            {
                return Forbid("Você não tem permissão para atualizar o perfil de outro usuário.");
            }
            
            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                user.Username = request.Username;
            }
            
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                user.Email = request.Email;
            }
            
            var passwordHelper = new PasswordHelper();
            passwordHelper.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }
            
            if (!string.IsNullOrWhiteSpace(request.Firstname))
            {
                user.Firstname = request.Firstname;
            }
            
            if (!string.IsNullOrWhiteSpace(request.Lastname))
            {
                user.Lastname = request.Lastname;
            }
            
            user.UpdatedAt = DateTime.UtcNow;

            _userRepository.UpdateUser(user);

            return Ok(user);
        }
        catch (System.Exception error)
        {
            return StatusCode(500, $"Ops... Não conseguimos atualizar seu usuário. Tente novamente!\nDetalhe do erro: {error.Message}");
        }
    }

    [Authorize]
    [HttpDelete("delete/{user}")]
    public IActionResult DeleteUser(string user)
    {
        try
        {
            var username = User.Identity.Name;
            
            if (user == username)
            {
                bool deleted = _userRepository.DeleteUser(user);
                
                if (deleted)
                {
                    return Ok("Usuário deletado com sucesso.");
                }
                else
                {
                    return NotFound("Usuário não encontrado.");
                }
            }
            else
            {
                return Forbid("Você não tem permissão para deletar o perfil de outro usuário.");
            }
        }
        catch (System.Exception error)
        {
            return StatusCode(500, $"Ops... Não conseguimos deletar seu perfil. Tente novamente!\nDetalhe do erro: {error.Message}");
        }
    }
}