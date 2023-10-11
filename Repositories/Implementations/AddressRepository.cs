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
                
                return (true, "Endereço atualizado.");
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

    public AddressModel UpdateAddress(AddressModel address)
    {
        _dataContext.Entry(address).State = EntityState.Modified;
        _dataContext.SaveChanges();

        return address;
    }

    public bool DeleteAddress(Guid id)
    {
        AddressModel address = GetAddressById(id);

        if (address == null)
        {
            throw new System.Exception("Houve um erro ao excluir o endereço.");
        }

        var local = _dataContext.Set<AddressModel>().Local.FirstOrDefault(entry => entry.Id.Equals(address.Id));
        if (local != null)
        {
            _dataContext.Entry(local).State = EntityState.Detached;
        }

        _dataContext.Addresses.Remove(address);
        _dataContext.SaveChanges();

        return true;
    }
}