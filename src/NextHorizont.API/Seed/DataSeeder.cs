using Microsoft.EntityFrameworkCore;
using NextHorizont.Application.Interfaces.Security;
using NextHorizont.Domain.Entities;
using NextHorizont.Infraestructura.Contexts;

namespace NextHorizont.Api.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();

        // La base de datos ya existe (Scaffolding), así que omitimos MigrateAsync()

        // Si ya hay tenants, no sembrar
        if (await context.Tenants.AnyAsync())
        {
            logger.LogInformation("La base de datos ya contiene datos. Seed omitido.");
            return;
        }

        logger.LogInformation("Sembrando datos iniciales...");

        // 1. Crear Tenant inicial
        var tenantId = Guid.NewGuid();
        var tenant = new Tenant(tenantId, "Mi Hotel & Restaurante", "hotel_restaurant");
        context.Tenants.Add(tenant);

        // 2. Crear Rol Admin
        var roleId = Guid.NewGuid();
        var adminRole = new Role(roleId, tenantId, "Admin", "[\"*\"]");
        context.Roles.Add(adminRole);

        // 3. Crear Usuario Admin
        var userId = Guid.NewGuid();
        var passwordHash = hasher.HashPassword("admin123");
        var adminUser = new User(userId, tenantId, roleId, "admin", passwordHash);
        context.Users.Add(adminUser);

        // 4. Crear métodos de pago básicos
        var efectivoId = Guid.NewGuid();
        var tarjetaId = Guid.NewGuid();
        context.PaymentMethods.Add(new PaymentMethod(efectivoId, tenantId, "Efectivo", false));
        context.PaymentMethods.Add(new PaymentMethod(tarjetaId, tenantId, "Tarjeta", true));

        await context.SaveChangesAsync();

        logger.LogInformation("Seed completado. TenantId: {TenantId} | Usuario: admin | Password: admin123", tenantId);
    }
}
