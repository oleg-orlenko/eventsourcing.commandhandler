using System;

namespace Orlenko.EventSourcing.Example.Domain.Events
{
    public abstract class BaseEvent<TEntity> : BaseEvent
        where TEntity : Entity
    {
        public readonly string UserName;

        public readonly TEntity Item;

        protected BaseEvent(string userName, TEntity item, Guid eventId, DateTime eventDate)
            : base(eventId, eventDate)
        {
            UserName = userName;
            Item = item ?? throw new ArgumentNullException(nameof(item));
        }
    }
}
