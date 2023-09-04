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
        List<AddressModel> addresses = _addressRepository.GetAllAddresses();
        return Ok(addresses);
    }

    [Authorize]
    [HttpPost("create-address")]
    public IActionResult CreateAddress([FromBody] CreateAddressDto request)
    {
        try
        {
            var address = new AddressModel();

            if (!string.IsNullOrEmpty(request.City))
            {
                address.City = request.City;
            }

            if (!string.IsNullOrEmpty(request.State))
            {
                address.State = request.State;
            }

            if (!string.IsNullOrEmpty(request.Country))
            {
                address.Country = request.Country;
            }
            
            var username = User.Identity.Name;

            var user = _userRepository.GetUserByUsername(username);
            if (user != null)
            {
                address.Username = username;
                address.User = user;
                _addressRepository.AddAddress(address);
                
                user.AddressId = address.Id;
                user.Address = address;
                _userRepository.UpdateUser(user);

                return Ok(address);
            }
            else
            {
                return BadRequest("Usuário não encontrado.");
            }
        }
        catch (System.Exception error)
        {
            return StatusCode(500, $"Ops... Não conseguimos cadastrar seu endereço. Tente novamente!\nDetalhe do erro: {error.Message}");
        }
    }

    [Authorize]
    [HttpPut("update-address")]
    public IActionResult UpdateAddress([FromBody] CreateAddressDto request)
    {
        try
        {
            var username = User.Identity.Name;
            
            var user = _userRepository.GetUserByUsername(username);

            if (user != null)
            {
                var address = _addressRepository.GetAllAddresses().FirstOrDefault(a => a.Username == username);

                if (address != null)
                {
                    if (!string.IsNullOrEmpty(request.City))
                    {
                        address.City = request.City;
                    }

                    if (!string.IsNullOrEmpty(request.State))
                    {
                        address.State = request.State;
                    }

                    if (!string.IsNullOrEmpty(request.Country))
                    {
                        address.Country = request.Country;
                    }
                    
                    address.UpdatedAt = DateTime.UtcNow;

                    _addressRepository.UpdateAddress(address);

                    var addressResponse = new UpdateAddress
                    {
                        City = address.City,
                        State = address.State,
                        Country = address.Country,
                        Username = username
                    };

                    return Ok(addressResponse);
                }
                else
                {
                    return BadRequest("Não foi possível atualizar o endereço. Endereço não encontrado.");
                }
            }
            else
            {
                return BadRequest("Não foi possível atualizar o endereço. Usuário não encontrado.");
            }
        }
        catch (System.Exception error)
        {
            return StatusCode(500, $"Ops... Não conseguimos atualizar seu endereço. Tente novamente!\nDetalhe do erro: {error.Message}");
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
                    return Ok("Endereço deletado com sucesso.");
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
            return StatusCode(500, $"Ops... Não conseguimos deleter seu endereço. Tente novamente!\nDetalhe do erro: {error.Message}");
        }
    }
}