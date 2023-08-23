using BlogAPIDotnet6.Data;
using BlogAPIDotnet6.Models;
using BlogAPIDotnet6.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogAPIDotnet6.Repositories.Implementations;

public class AddressRepository : IAddressRepository
{
    private readonly DataContext _dataContext;

    public AddressRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public AddressModel GetAddressById(Guid id)
    {
        return _dataContext.Addresses.FirstOrDefault(x => x.Id == id);
    }

    public List<AddressModel> GetAllAddresses()
    {
        return _dataContext.Addresses.ToList();
    }

    public AddressModel AddAddress(AddressModel address)
    {
        _dataContext.Addresses.Add(address);
        _dataContext.SaveChanges();
        return address;
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

        _dataContext.Addresses.Remove(address);
        _dataContext.SaveChanges();

        return true;
    }
}