using System.Reflection;
using FluentValidation;
using Friday.Application.Common.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Friday.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, Assembly assembly)
    {
        services.AddMediatR(assembly);
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
        services.AddValidatorsFromAssembly(assembly);
        services.AddAutoMapper(assembly);
        
        return services;
    }
}