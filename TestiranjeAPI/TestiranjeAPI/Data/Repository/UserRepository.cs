using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TestiranjeAPI.Models;
using TestiranjeAPI.Repository.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace TestiranjeAPI.Repository;

public class UserRepository : Repository<User>, IUserRepository
{

    public UserRepository(PartyContext context) : base(context) { }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        var user = await _context.Users
            .Where(u => u.Username == username)
            .FirstOrDefaultAsync();

        return user;
    }

    public async Task<User?> GetUserByUsernameAndPasswordAsync(string username, string password)
    {
        var user = await _context.Users
            .Where(u => u.Username == username && u.Password == password)
            .FirstOrDefaultAsync();

        return user;
    }

    public async Task<UserViewModel> GetUserInfoAsync(int userId)
    {
        var existingUser = await GetByIdAsync(userId);

        return new UserViewModel(
            existingUser!.Username,
            existingUser!.Email,
            existingUser!.Password,
            existingUser!.Avatar);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
        return;
    }

}
