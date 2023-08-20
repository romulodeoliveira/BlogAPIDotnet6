using BlogAPIDotnet6.Models;

namespace BlogAPIDotnet6.Repositories.Interfaces;

public interface IUserRepository
{
    UserModel ListUserById(Guid id);
    List<UserModel> GetAllUsers();
    UserModel AddUser(UserModel user);
    UserModel UpdateUser(UserModel user);
    bool DeleteUser(Guid id);
}