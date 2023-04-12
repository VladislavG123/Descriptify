using Descriptify.Dal.Entities;
using Descriptify.Dal.Providers.Abstract;

namespace Descriptify.Dal.Providers;

public interface IUserProvider : ICrudProvider<UserEntity>
{
    public Task<UserEntity?> GetByLoginOrDefault(string login);
}