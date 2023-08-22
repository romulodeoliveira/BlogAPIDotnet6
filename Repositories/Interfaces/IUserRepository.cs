using BlogAPIDotnet6.Models;

namespace BlogAPIDotnet6.Repositories.Interfaces;

public interface IUserRepository
{
    UserModel GetUserByUsername(string username);
    List<UserModel> GetAllUsers();
    UserModel AddUser(UserModel user);
    UserModel UpdateUser(UserModel user);
    bool DeleteUser(string username);
}