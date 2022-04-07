﻿using System.Text;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Friday.Domain.Infrastructure.MessageBrokers;

namespace Friday.Infrastructure.MessageBrokers.AzureServiceBus;

public class AzureServiceBusMessageReceivedArgs<T> : EventArgs
{
    public T Data { get; set; }
    public MetaData MetaData { get; set; }
}

public class AzureServiceBusReceiver<T> : IMessageReceiver
{
    private readonly string _connectionString;
    protected readonly string _queueName;

    private const int Timeout = 1000;

    public event EventHandler<AzureServiceBusMessageReceivedArgs<T>> MessageReceived;
    
    public AzureServiceBusReceiver(string connectionString, string queueName)
    {
        _connectionString = connectionString;
        _queueName = queueName;
    }
    
    public void Receive(Action<T, MetaData> action)
    {
        Task.Factory.StartNew(() => ReceiveAsync(action));
    }

    private Task ReceiveAsync(Action<T, MetaData> action)
    {
        return ReceiveStringAsync(retrievedMessage =>
        {
            var message = JsonSerializer.Deserialize<Message<T>>(retrievedMessage);
            action?.Invoke(message!.Data, message.MetaData);
        });
    }
    
    public void ReceiveString(Action<string> action)
    {
        Task.Factory.StartNew(() => ReceiveStringAsync(action));
    }

    private async Task ReceiveStringAsync(Action<string> action)
    {
        await using var client = new ServiceBusClient(_connectionString);
        var receiver = CreateReceiver(client);

        while (true)
        {
            var retrievedMessage = await receiver.ReceiveMessageAsync();

            if (retrievedMessage is not null)
            {
                action(Encoding.UTF8.GetString(retrievedMessage.Body));
                await receiver.CompleteMessageAsync(retrievedMessage);
            }
            else
            {
                await Task.Delay(1000);
            }
        }
    }

    protected virtual ServiceBusReceiver CreateReceiver(ServiceBusClient client)
    {
        return client.CreateReceiver(_queueName);
    }
}