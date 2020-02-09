using System;

namespace Orlenko.EventSourcing.Example.Domain.Events
{
    public class ItemDeletedEvent : BaseEvent<Item>
    {
        public ItemDeletedEvent(Item item, string userName, Guid eventId, DateTime eventDate)
            : base(userName, item, eventId, eventDate)
        {
        }
    }
}
