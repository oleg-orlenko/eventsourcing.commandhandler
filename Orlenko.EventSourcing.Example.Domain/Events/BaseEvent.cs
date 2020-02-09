using System;

namespace Orlenko.EventSourcing.Example.Domain.Events
{
    public abstract class BaseEvent
    {
        protected BaseEvent(Guid eventId, DateTime eventDate)
        {
            EventDate = eventDate;
            EventId = eventId;
        }

        public readonly DateTime EventDate;

        public readonly Guid EventId;
    }
}
