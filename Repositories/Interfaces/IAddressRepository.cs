using BlogAPIDotnet6.DTOs;
using BlogAPIDotnet6.Models;

namespace BlogAPIDotnet6.Repositories.Interfaces;

public interface IAddressRepository
{
    AddressModel GetAddressById(Guid id);
    List<AddressModel> GetAllAddresses();
    (bool Success, string Message) AddAddress(CreateAddressDto address, string username);
    (bool Success, string Message) UpdateAddress(UpdateAddressDto addressDto, string username);
    (bool Success, string Message) DeleteAddress(Guid addressId, string username);
}