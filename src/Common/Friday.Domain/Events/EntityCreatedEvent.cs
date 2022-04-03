using Friday.Domain.Entities;

namespace Friday.Domain.Events;

public class EntityCreatedEvent<T> : IDomainEvent 
    where T : Entity<T>
{
    public T Entity { get; set; }
    public DateTime EventDateTime { get; set; }
    
    public EntityCreatedEvent(T entity, DateTime dateTime)
    {
        Entity = entity;
        EventDateTime = dateTime;
    }
}