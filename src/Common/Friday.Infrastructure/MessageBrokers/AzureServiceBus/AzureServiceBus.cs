using System.Reflection;
using Friday.Domain.Events;
using Friday.Domain.Infrastructure.MessageBrokers;
using Microsoft.Extensions.DependencyInjection;

namespace Friday.Infrastructure.MessageBrokers.AzureServiceBus;

public static class AzureServiceBus
{
    public static IServiceProvider Provider { get; set; }
    
    public static void RegisterAzureServiceBusSubscriptionReceivers(
        string connectionString, string topicName, string subscriptionName)
    {
        var collection = DomainEvents.ServiceCollection!;
        var method = typeof(AzureServiceBus).GetMethod(nameof(CreateInstance), BindingFlags.Static | BindingFlags.NonPublic);
        
        foreach (var type in DomainEvents.Handlers!)
        {
            var interfaces = type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>))
                .ToList();

            foreach (var i in interfaces)
            {
                var eventType = i.GetGenericArguments()[0];

                var genericMethod = method!.MakeGenericMethod(eventType);
                var receiver = genericMethod.Invoke(null, new object?[] {connectionString, topicName, subscriptionName});

                collection.AddSingleton(receiver);
            }
        }
    }

    private static IMessageReceiver CreateInstance<T>(
        string connectionString, string topicName, string subscriptionName)
        where T : IDomainEvent
    {
        var receiver = new AzureServiceBusSubscriptionReceiver<T>(connectionString, topicName, subscriptionName);
        receiver.Receive((data, metaData) =>
        {
            var handler = Provider.GetService<IDomainEventHandler<T>>();
            handler!.HandleAsync(data);
        });

        return receiver;
    }
}