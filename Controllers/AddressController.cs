using BlogAPIDotnet6.DTOs;
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
    [HttpDelete("delete/{addressId}")]
    public IActionResult DeleteUser([FromRoute] Guid addressId)
    {
        try
        {
            var username = User.Identity.Name;
            var deleted = _addressRepository.DeleteAddress(addressId, username);

            if (deleted.Success)
            {
                return Ok(deleted.Message);
            }
            else
            {
                return NotFound(deleted.Message);
            }
        }
        catch (System.Exception error)
        {
            return StatusCode(500, $"Erro interno do servidor: {error.Message}");
        }
    }
}