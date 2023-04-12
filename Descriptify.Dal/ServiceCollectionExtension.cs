using Descriptify.Dal.Providers;
using Descriptify.Dal.Providers.EntityFramework;
using Microsoft.Extensions.DependencyInjection;

namespace Descriptify.Dal;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDataAccessLayout(this IServiceCollection serviceCollection) 
        => serviceCollection
            .AddScoped<IUserProvider, UserEfProvider>();
}