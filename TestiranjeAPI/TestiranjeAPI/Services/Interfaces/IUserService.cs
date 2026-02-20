using TestiranjeAPI.Models;

namespace TestiranjeAPI.Services.Interfaces;

public interface IUserService
{
    public Task<int> Login(UserLogin userLogin);
    public Task<User> Register(UserRegister userRegister);
    public Task<User> UpdateUser(int userId, UserUpdate userUpdate);
    public Task<UserViewModel> GetUserInfo(int id);
}