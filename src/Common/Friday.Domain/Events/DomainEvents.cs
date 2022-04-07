using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Friday.Domain.Events;

public class DomainEvents : IDomainEvents
{
    private static IServiceProvider _serviceProvider;
    public static IServiceCollection? ServiceCollection { get; private set; }
    public static List<Type>? Handlers { get; private set; }

    public static void RegisterHandlersFromAssembly(Assembly assembly, IServiceCollection serviceCollection)
    {
        Handlers = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)))
            .ToList();

        foreach (var handler in Handlers)
        {
            var interfaces = handler.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>))
                .ToList();
            var type = interfaces[0];
            serviceCollection.AddTransient(type, handler);
        }

        ServiceCollection = serviceCollection;
    }

    public DomainEvents(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        foreach (var handlerType in Handlers!)
        {
            bool canHandleEvent = handlerType.GetInterfaces()
                .Any(x => x.IsGenericType
                          && x.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)
                          && x.GenericTypeArguments[0] == domainEvent.GetType());

            if (canHandleEvent)
            {
                dynamic handler = _serviceProvider.GetService(handlerType)!;
                await handler.HandleAsync((dynamic)domainEvent, cancellationToken);
            }
        }
    }
}