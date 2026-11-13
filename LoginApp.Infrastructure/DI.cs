using LoginApp.Application;
using LoginApp.Application.Common;
using LoginApp.Application.Users;
using LoginApp.Domain.Common;
using LoginApp.Domain.Repositories;
using LoginApp.Infrastructure.Persistence;
using LoginApp.Infrastructure.Persistence.Events;
using LoginApp.Infrastructure.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LoginApp.Infrastructure;

public static class LoginAppDependencies
{
    public static IServiceCollection AddLoginAppDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLoginApplication();

        var connectionString = configuration.GetConnectionString("LoginAppDb")
            ?? throw new InvalidOperationException("ConnectionStrings:loginappdb is not set.");
        services.AddDbContext<LoginAppDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IUserUnitOfWork, UserUnitOfWork>();
        services.AddScoped<IDomainEventRecorder, DomainEventRecorder>();
        services.AddSingleton<IClock, SystemClock>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserQueries, UserQueries>();

        return services;
    }
}
