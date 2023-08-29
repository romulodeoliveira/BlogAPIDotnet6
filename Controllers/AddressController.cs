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
            var address = new AddressModel();
            var addressResponse = new UpdateAddress();

            if (!string.IsNullOrEmpty(request.City))
            {
                address.City = request.City;
                addressResponse.City = address.City;
            }

            if (!string.IsNullOrEmpty(request.State))
            {
                address.State = request.State;
                addressResponse.State = address.State;
            }

            if (!string.IsNullOrEmpty(request.Country))
            {
                address.Country = request.Country;
                addressResponse.Country = address.Country;
            }

            addressResponse.Username = username;
            
            if (user != null)
            {
                _addressRepository.UpdateAddress(address);

                return Ok(addressResponse);
            }
            else
            {
                return BadRequest("Não foi possivel atualizar o endereço. Usuário não encontrado.");
            }
        }
        catch (System.Exception error)
        {
            return StatusCode(500, $"Ops... Não conseguimos atualizar seu usuário. Tente novamente!\nDetalhe do erro: {error.Message}");
        }
    }
}