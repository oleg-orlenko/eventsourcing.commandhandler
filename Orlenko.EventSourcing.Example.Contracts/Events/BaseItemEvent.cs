using System;

namespace Orlenko.EventSourcing.Example.Contracts.Events
{
    public class BaseItemEvent : BaseEvent
    {
        public readonly string UserName;

        public readonly Guid ItemId; 

        public BaseItemEvent(string userName, Guid itemId, Guid eventId, DateTime eventDate) : base(eventId, eventDate)
        {
            this.UserName = userName;
            this.ItemId = itemId;
        }
    }
}
