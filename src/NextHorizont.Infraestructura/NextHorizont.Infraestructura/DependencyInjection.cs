using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NextHorizont.Domain.Interfaces;
using NextHorizont.Infraestructura.Caching;
using NextHorizont.Infraestructura.Persistence;
using NextHorizont.Infraestructura.Repositories;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace NextHorizont.Infraestructura;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraestructura(this IServiceCollection services, IConfiguration configuration)
    {
        // Entity Framework Core
        services.AddDbContext<NextHorizont.Infraestructura.Contexts.AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Redis
        var redisConnectionString = configuration.GetConnectionString("RedisConnection") ?? "localhost:6379";
        services.AddSingleton<IConnectionMultiplexer>(sp => 
            ConnectionMultiplexer.Connect(redisConnectionString));
            
        services.AddSingleton<ICacheService, RedisCacheService>();

        // Dapper Connection Factory
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();

        // Repositories
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IGuestRepository, GuestRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IStayRepository, StayRepository>();
        services.AddScoped<ICashShiftRepository, CashShiftRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();
        
        return services;
    }
}
