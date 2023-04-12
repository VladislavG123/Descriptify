using Descriptify.Bll.Abstract;
using Descriptify.Bll.Dtos;
using Descriptify.Dal.Providers;

namespace Descriptify.Bll;

public class UserService : IUserService
{
    private readonly IUserProvider _userProvider;

    public UserService(IUserProvider userProvider)
    {
        _userProvider = userProvider;
    }

    public async Task<UserDto> GetUserById(Guid id)
    {
        var user = await _userProvider.GetById(id);

        return new UserDto(user.Id, user.Login, user.Username);
    }

    public async Task<UserDto> GetUserByLogin(string login)
    {
        var user = await _userProvider.GetByPredicate(x => x.Login == login);
        
        return new UserDto(user.Id, user.Login, user.Username);
    }
}