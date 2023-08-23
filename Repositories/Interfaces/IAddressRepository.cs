using BlogAPIDotnet6.Models;

namespace BlogAPIDotnet6.Repositories.Interfaces;

public interface IAddressRepository
{
    AddressModel GetAddressById(Guid id);
    List<AddressModel> GetAllAddresses();
    AddressModel AddAddress(AddressModel address);
    AddressModel UpdateAddress(AddressModel address);
    bool DeleteAddress(Guid id);
}