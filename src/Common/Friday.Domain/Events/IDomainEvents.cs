namespace Friday.Domain.Events;

public interface IDomainEvents
{
    public Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
}