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

    public AddressController(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
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

            _addressRepository.AddAddress(address);
            
            return Ok(address);
        }
        catch (System.Exception error)
        {
            return StatusCode(500, $"Ops... Não conseguimos cadastrar seu endereço. Tente novamente!\nDetalhe do erro: {error.Message}");
        }
    }
}