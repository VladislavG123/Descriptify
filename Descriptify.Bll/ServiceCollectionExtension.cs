using Descriptify.Bll.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Descriptify.Bll;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddBusinessLoginLayout(this IServiceCollection serviceCollection)
        => serviceCollection.AddScoped<IAuthenticateService, AuthenticateService>();
}