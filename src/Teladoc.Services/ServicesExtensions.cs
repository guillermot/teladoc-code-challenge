using Microsoft.Extensions.DependencyInjection;
using Teladoc.DataAccess;
using Teladoc.Services.Interface;
using Teladoc.Services.Mappers;
using Teladoc.Services.Services;

namespace Teladoc.Services;

public static class ServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddDataAccess("TeladocDb");
        services.AddScoped<IUserService, UserService>();
        services.AddAutoMapper(typeof(UserMappingProfile));
        
        return services;
    }
}