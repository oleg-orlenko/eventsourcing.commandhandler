using System;

namespace Orlenko.EventSourcing.Example.Domain.Events
{
    public class ItemCreatedEvent : BaseEvent<Item>
    {
        public ItemCreatedEvent(Item item, string userName, Guid eventId, DateTime eventDate)
            : base(userName, item, eventId, eventDate)
        {
        }
    }
}
