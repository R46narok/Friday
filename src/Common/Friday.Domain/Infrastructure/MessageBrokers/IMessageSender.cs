namespace Friday.Domain.Infrastructure.MessageBrokers;

public interface IMessageSender<T> 
{
    public Task SendAsync(T message, MetaData metaData, CancellationToken cancellationToken);
}