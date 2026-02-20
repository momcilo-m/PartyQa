using AutoMapper;
using TestiranjeAPI.Services.Interfaces;
using TestiranjeAPI.Models;
using TestiranjeAPI.Repository;
using TestiranjeAPI.Repository.Interfaces;

namespace TestiranjeAPI.Services;

public class UserService : IUserService
{

    private IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository uRepo, IMapper mapper)
    {
        _userRepository = uRepo;
        _mapper = mapper;
    }

    public async Task<int> Login(UserLogin userLogin)
    {
        var existingUser = await _userRepository.GetUserByUsernameAndPasswordAsync(userLogin.Username, userLogin.Password);
        if(existingUser == null)
            throw new Exception("User not found");
        return existingUser.Id;
    }

    public async Task<User> Register(UserRegister userRegister)
    {
        var existingUser = await _userRepository.GetUserByUsernameAsync(userRegister.Username);

        if (existingUser != null) throw new Exception("User exist");

        User user = _mapper.Map<User>(userRegister);

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return user;
    }

    public async Task<User> UpdateUser(int userId, UserUpdate userUpdate)
    {
        var userToUpdate = await _userRepository.GetByIdAsync(userId);

        if (userToUpdate == null)
            throw new Exception("User not found");

        var existingUser = await _userRepository.GetUserByUsernameAsync(userUpdate.Username);

        if(existingUser != null && userUpdate.Username != userToUpdate.Username) 
            throw new Exception("User with this username already exist");

        _mapper.Map(userUpdate, userToUpdate);

        _userRepository.Update(userToUpdate);
        await _userRepository.SaveChangesAsync();

        return userToUpdate;
    }


    public async Task<UserViewModel> GetUserInfo(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null) throw new Exception("User not found");

        return _mapper.Map<UserViewModel>(user);

    }

}