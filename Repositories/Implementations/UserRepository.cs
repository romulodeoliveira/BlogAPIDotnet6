using BlogAPIDotnet6.Data;
using BlogAPIDotnet6.Models;
using BlogAPIDotnet6.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogAPIDotnet6.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly DataContext _dataContext;

    public UserRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public UserModel GetUserByUsername(string username)
    {
        return _dataContext.Users.FirstOrDefault(u => u.Username == username);
    }

    public List<UserModel> GetAllUsers()
    {
        return _dataContext.Users.ToList();
    }

    public UserModel AddUser(UserModel user)
    {
        _dataContext.Users.Add(user);
        _dataContext.SaveChanges();
        return user;
    }

    public UserModel UpdateUser(UserModel user)
    {
        _dataContext.Entry(user).State = EntityState.Modified;
        _dataContext.SaveChanges();

        return user;
    }

    public bool DeleteUser(string username)
    {
        UserModel user = GetUserByUsername(username);

        if (user == null)
        {
            throw new System.Exception("Houve um erro ao excluir o usuário.");
        }
        
        if (user.AddressId != null)
        {
            var address = _dataContext.Addresses.FirstOrDefault(a => a.Id == user.AddressId);

            if (address != null)
            {
                _dataContext.Addresses.Remove(address);
            }
        }

        _dataContext.Users.Remove(user);
        _dataContext.SaveChanges();

        return true;
    }
}