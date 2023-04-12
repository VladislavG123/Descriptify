using Descriptify.Bll.Dtos;

namespace Descriptify.Bll.Abstract;

public interface IUserService
{
    Task<UserDto> GetUserById(Guid id);
    Task<UserDto> GetUserByLogin(string login);
}