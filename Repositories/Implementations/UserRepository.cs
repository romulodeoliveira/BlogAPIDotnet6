using BlogAPIDotnet6.Data;
using BlogAPIDotnet6.Models;
using BlogAPIDotnet6.Repositories.Interfaces;

namespace BlogAPIDotnet6.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly DataContext _dataContext;

    public UserRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public UserModel ListUserById(Guid id)
    {
        return _dataContext.Users.FirstOrDefault(x => x.Id == id);
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
        UserModel user1 = ListUserById(user.Id);

        if (user1 == null)
        {
            throw new System.Exception("Houve um erro ao atualizar o usuário.");
        }

        user1.Username = user.Username;
        user1.Firstname = user.Firstname;
        user1.Lastname = user.Lastname;

        _dataContext.Users.Update(user1);
        _dataContext.SaveChanges();

        return user1;
    }

    public bool DeleteUser(Guid id)
    {
        UserModel user = ListUserById(id);

        if (user == null)
        {
            throw new System.Exception("Houve um erro ao excluir o usuário.");
        }

        _dataContext.Users.Remove(user);
        _dataContext.SaveChanges();

        return true;
    }
}