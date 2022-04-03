namespace Friday.Domain.Entities;

public abstract class Entity<TKey> : IEntity<TKey>
{
    public TKey Id { get; set; }

    public DateTime CreatedDateTime { get; set; }
    public DateTime UpdatedDateTime { get; set; }
}