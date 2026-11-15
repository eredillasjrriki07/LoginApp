using FluentValidation;
using LoginApp.Application.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace LoginApp.Application;

public static class LoginApplication
{
    public static IServiceCollection AddLoginApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(LoginApplication).Assembly);
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }
}
