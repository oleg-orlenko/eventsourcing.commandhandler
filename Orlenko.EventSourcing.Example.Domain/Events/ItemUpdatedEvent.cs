using System;

namespace Orlenko.EventSourcing.Example.Domain.Events
{
    public class ItemUpdatedEvent : BaseEvent<Item>
    {
        public ItemUpdatedEvent(Item item, string userName, Guid eventId, DateTime eventDate)
            : base(userName, item, eventId, eventDate)
        {
        }
    }
}
