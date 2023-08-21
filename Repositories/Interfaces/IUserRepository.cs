using BlogAPIDotnet6.Models;

namespace BlogAPIDotnet6.Repositories.Interfaces;

public interface IUserRepository
{
    UserModel ListUserById(Guid id);
    UserModel GetUserByUsername(string username);
    List<UserModel> GetAllUsers();
    UserModel AddUser(UserModel user);
    UserModel UpdateUser(UserModel user);
    bool DeleteUser(Guid id);
}