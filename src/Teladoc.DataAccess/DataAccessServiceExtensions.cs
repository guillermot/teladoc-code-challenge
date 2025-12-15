using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Teladoc.DataAccess.Interface;
using Teladoc.DataAccess.Repositories;

namespace Teladoc.DataAccess;

public static class DataAccessServiceExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, string inMemoryDbName)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseInMemoryDatabase(inMemoryDbName);
        });

        services.AddScoped<IUserRepository, UserRepository>();
        
        return services;
    }
}