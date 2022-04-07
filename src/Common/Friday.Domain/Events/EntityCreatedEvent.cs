using Friday.Domain.Entities;

namespace Friday.Domain.Events;

public class EntityCreatedEvent<T> : IDomainEvent
{
    public T Entity { get; set; }
    public DateTime EventDateTime { get; set; }

    public EntityCreatedEvent()
    {
        
    }
    
    public EntityCreatedEvent(T entity, DateTime dateTime)
    {
        Entity = entity;
        EventDateTime = dateTime;
    }
}