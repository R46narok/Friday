namespace Friday.Domain.Infrastructure.MessageBrokers;

public class Message<T>
{
    public T Data { get; set; }
    public MetaData MetaData { get; set; }
}