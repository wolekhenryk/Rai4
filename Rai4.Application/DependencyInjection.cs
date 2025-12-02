using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rai4.Application.Background;
using Rai4.Application.Services.Implementation;
using Rai4.Application.Services.Interfaces;
using Rai4.Infrastructure.Options;

namespace Rai4.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddAutoMapper(assembly);

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
        });

        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<ICurrentUserService, CurrentUSerService>();
        services.AddScoped<IStopTrackingService, StopTrackingService>();

        // Register ConnectedClientManager as Singleton to maintain state across all connections
        services.AddSingleton<IConnectedClientManager, ConnectedClientManager>();

        services.AddSignalR();
        
        services.AddMemoryCache();
        
        services.Configure<ZtmOptions>(configuration.GetSection("Ztm"));
        
        services.AddHttpClient<IZtmClient, ZtmClient>()
            .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(15),
                MaxConnectionsPerServer = 10
            });

        // Register background service for polling and pushing updates
        services.AddHostedService<BackgroundSignalRPush>();

        return services;
    }
}
