using BlogAPIDotnet6.DTOs.User;
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

    [HttpGet("list-users")]
    public IActionResult ListAllUserInformation()
    {
        List<UserModel> users = _userRepository.GetAllUsers();
        List<PublicInfo> sortedUsers = users
            .OrderBy(c => c.Username)
            .Select(c => new PublicInfo()
            {
                Username = c.Username,
                Email = c.Email,
                Firstname = c.Firstname,
                Lastname = c.Lastname,
                AddressId = c.AddressId
            })
            .ToList();

        return Ok(sortedUsers);
    }

    [Authorize]
    [HttpGet("get-own-user-information")]
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

            var userInfo = new
            {
                user.Username,
                user.Email,
                user.Firstname,
                user.Lastname,
                user.Role,
                user.AddressId,
                user.CreatedAt,
                user.UpdatedAt
            };
            
            return Ok(userInfo);
        }
        catch (System.Exception error)
        {
            return StatusCode(500, $"Erro interno: {error.Message}");
        }
    }

    [HttpPost("register")]
    public IActionResult Register(Register request)
    {
        try
        {
            var existingUser = _userRepository.GetUserByUsername(request.Username);

            if (existingUser != null)
            {
                return BadRequest("Nome de usuário já está em uso.");
            }

            var passwordHelper = new PasswordHelper();

            if (!passwordHelper.CheckerStrongPassword(request.Password))
            {
                return BadRequest(
                    "Senha não aceita. Insira uma senha de 6 a 12 caracteres e que tenha pelo menos uma letra maiuscula, uma minuscula, um numero e um caractere especial.");
            }

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

            return Ok("Usuário cadastrado com sucesso.");
        }
        catch (Exception error)
        {
            return StatusCode(500, $"Erro interno: {error.Message}");
        }
    }
    
    [HttpPost("login")]
    public IActionResult Login(Login request)
    {
        try
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
        catch (Exception error)
        {
            return StatusCode(500, $"Erro interno: {error.Message}");
        }
    }
    
    [Authorize]
    [HttpPut("update-profile")]
    public IActionResult UpdateProfile([FromBody] Update request)
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

            return Ok("Usuário atualizado com sucesso.");
        }
        catch (System.Exception error)
        {
            return StatusCode(500, $"Erro interno: {error.Message}");
        }
    }

    [Authorize]
    [HttpDelete("delete/{user}")]
    public IActionResult DeleteUser([FromRoute] string user)
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
            return StatusCode(500, $"Erro interno: {error.Message}");
        }
    }
}