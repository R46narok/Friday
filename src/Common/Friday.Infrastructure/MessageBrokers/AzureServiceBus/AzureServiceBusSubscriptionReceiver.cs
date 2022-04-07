using Azure.Messaging.ServiceBus;

namespace Friday.Infrastructure.MessageBrokers.AzureServiceBus;

public class AzureServiceBusSubscriptionReceiver<T> : AzureServiceBusReceiver<T>
{
    private readonly string _subscriptionName;
    
    public AzureServiceBusSubscriptionReceiver(string connectionString, string topicName, string subscriptionName) 
        : base(connectionString, topicName)
    {
        _subscriptionName = subscriptionName;
    }

    protected override ServiceBusReceiver CreateReceiver(ServiceBusClient client)
    {
        return client.CreateReceiver(_queueName, _subscriptionName);
    }
}