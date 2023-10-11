using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using BlogAPIDotnet6.DTOs;
using BlogAPIDotnet6.Models;
using BlogAPIDotnet6.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPIDotnet6.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressController : ControllerBase
{
    private readonly IAddressRepository _addressRepository;
    private readonly IUserRepository _userRepository;

    public AddressController(IAddressRepository addressRepository, IUserRepository userRepository)
    {
        _addressRepository = addressRepository;
        _userRepository = userRepository;
    }

    [Authorize]
    [HttpGet("list-address")]
    public IActionResult ListAddress()
    {
        var response = _addressRepository.GetAllAddresses();
            
        return Ok(response);
    }

    [Authorize]
    [HttpPost("create-address")]
    public IActionResult CreateAddress([FromBody] CreateAddressDto request)
    {
        try
        {
            var username = User.Identity.Name;
            var response = _addressRepository.AddAddress(request, username);

            if (response.Success)
            {
                return Ok(response.Message);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }
        catch (System.Exception error)
        {
            return StatusCode(500, $"Erro interno do servidor: {error.Message}");
        }
    }

    [Authorize]
    [HttpPut("update-address")]
    public IActionResult UpdateAddress([FromBody] UpdateAddressDto request)
    {
        try
        {
            var username = User.Identity.Name;
            var response = _addressRepository.UpdateAddress(request, username);
            
            if (response.Success)
            {
                return Ok(response.Message);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }
        catch (System.Exception error)
        {
            return StatusCode(500, $"Erro interno do servidor: {error.Message}");
        }
    }

    [Authorize]
    [HttpDelete("delete")]
    public IActionResult DeleteUser()
    {
        try
        {
            var username = User.Identity.Name;
            
            var user = _userRepository.GetUserByUsername(username);

            if (user.AddressId != null)
            {
                bool deleted = _addressRepository.DeleteAddress(user.AddressId.Value);

                if (deleted)
                {
                    return Ok();
                }
                else
                {
                    return NotFound("Endereço não encontrado.");
                } 
            }
            else
            {
                return BadRequest("Não foi possível deletar o endereço. Nenhum endereço encontrado.");
            }
        }
        catch (System.Exception error)
        {
            return StatusCode(500, $"Erro interno do servidor: {error.Message}");
        }
    }
}