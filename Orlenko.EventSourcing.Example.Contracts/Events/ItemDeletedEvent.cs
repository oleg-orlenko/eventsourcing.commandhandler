using System;

namespace Orlenko.EventSourcing.Example.Contracts.Events
{
    public class ItemDeletedEvent : BaseItemEvent
    {
        public ItemDeletedEvent(Guid itemId, string userName, Guid eventId, DateTime eventDate, int version = 0) : base(userName, itemId, eventId, eventDate, version)
        {
        }
    }
}
