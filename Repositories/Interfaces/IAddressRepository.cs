using BlogAPIDotnet6.DTOs;
using BlogAPIDotnet6.Models;

namespace BlogAPIDotnet6.Repositories.Interfaces;

public interface IAddressRepository
{
    AddressModel GetAddressById(Guid id);
    List<AddressModel> GetAllAddresses();
    (bool Success, string Message) AddAddress(CreateAddressDto address, string username);
    AddressModel UpdateAddress(AddressModel address);
    bool DeleteAddress(Guid id);
}