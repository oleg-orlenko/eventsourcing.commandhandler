using System;

namespace Orlenko.EventSourcing.Example.Contracts.Events
{
    public abstract class BaseItemEvent : BaseEvent
    {
        public readonly string UserName;

        public readonly Guid ItemId; 

        protected BaseItemEvent(string userName, Guid itemId, Guid eventId, DateTime eventDate, int version) : base(eventId, eventDate, version)
        {
            this.UserName = userName;
            this.ItemId = itemId;
        }
    }
}
