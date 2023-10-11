using BlogAPIDotnet6.Data;
using BlogAPIDotnet6.DTOs;
using BlogAPIDotnet6.Models;
using BlogAPIDotnet6.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogAPIDotnet6.Repositories.Implementations;

public class AddressRepository : IAddressRepository
{
    private readonly DataContext _dataContext;
    private readonly IUserRepository _userRepository;

    public AddressRepository(DataContext dataContext, IUserRepository userRepository)
    {
        _dataContext = dataContext;
        _userRepository = userRepository;
    }
    
    public AddressModel GetAddressById(Guid id)
    {
        var response = _dataContext.Addresses.FirstOrDefault(x => x.Id == id);
        
        if (response == null)
        {
            return null;
        }

        return new AddressModel()
        {
            Id = response.Id,
            Country = response.Country,
            State = response.State,
            City = response.City,
            Username = response.Username,
            CreatedAt = response.CreatedAt,
            UpdatedAt = response.UpdatedAt
        };
    }

    public List<AddressModel> GetAllAddresses()
    {
        return _dataContext.Addresses.ToList();
    }

    public (bool Success, string Message) AddAddress(CreateAddressDto request, string username)
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

            var user = _userRepository.GetUserByUsername(username);

            if (user != null)
            {
                address.Username = username;
                address.User = user;
                
                _dataContext.Addresses.Add(address);
                _dataContext.SaveChanges();
                
                return (true, "Endereço cadastrado.");
            }
            else
            {
                return (false, "Usuário não encontraro.");
            }
        }
        catch (Exception error)
        {
            return (false, $"Erro interno do servidor: {error.Message}");
        }
    }

    public (bool Success, string Message) UpdateAddress(UpdateAddressDto request, string username)
    {
        try
        {
            var user = _userRepository.GetUserByUsername(username);

            if (user == null)
            {
                return (false, "Usuário não encontrado.");
            }

            var local = GetAddressById(user.AddressId.Value);

            if (!string.IsNullOrEmpty(request.City))
            {
                local.City = request.City;
            }

            if (!string.IsNullOrEmpty(request.State))
            {
                local.State = request.State;
            }

            if (!string.IsNullOrEmpty(request.Country))
            {
                local.Country = request.Country;
            }

            local.UpdatedAt = DateTime.UtcNow;

            _dataContext.SaveChanges();

            return (true, "Endereço atualizado.");
        }
        catch (Exception error)
        {
            return (false, $"Erro interno do servidor: {error.Message}");
        }
    }

    public (bool Success, string Message) DeleteAddress(Guid addressId, string username)
    {
        try
        {
            var address = GetAddressById(addressId);
            var user = _userRepository.GetUserByUsername(username);
            
            var local = GetAddressById(address.Id);
            if (user != null)
            {
                if (local != null)
                {
                    _dataContext.Addresses.Remove(address);
                    _dataContext.SaveChanges();

                    return (true, "Endereço excluído com sucesso.");
                }
                else
                {
                    return (false, "Endereço não encontrado.");
                }
            }
            else
            {
                return (false, "Usuário não encontrado.");
            }
        }
        catch (Exception error)
        {
            return (false, $"Erro interno do servidor: {error.Message}");
        }
    }
}