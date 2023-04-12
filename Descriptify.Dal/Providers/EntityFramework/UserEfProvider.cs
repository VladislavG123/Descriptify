using Descriptify.Dal.Entities;

namespace Descriptify.Dal.Providers.EntityFramework;

public class UserEfProvider : BaseEfProvider<UserEntity>, IUserProvider
{
    public UserEfProvider(ApplicationContext context) : base(context)
    {
    }

    public async Task<UserEntity?> GetByLoginOrDefault(string login)
        => await GetByPredicateOrDefault(x => x.Login.Equals(login));
}