using System;

namespace Orlenko.EventSourcing.Example.Contracts.Events
{
    public class ItemDeletedEvent : BaseItemEvent
    {
        public ItemDeletedEvent(Guid itemId, string userName, Guid eventId, DateTime eventDate) : base(userName, itemId, eventId, eventDate)
        {
        }
    }
}
