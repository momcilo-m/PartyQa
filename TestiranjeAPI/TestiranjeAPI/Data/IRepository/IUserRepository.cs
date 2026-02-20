using TestiranjeAPI.Models;
using Task = System.Threading.Tasks.Task;

namespace TestiranjeAPI.Repository.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User?> GetUserByUsernameAndPasswordAsync(string username, string password);
    Task<UserViewModel> GetUserInfoAsync(int userId);
    Task SaveChangesAsync();
}
