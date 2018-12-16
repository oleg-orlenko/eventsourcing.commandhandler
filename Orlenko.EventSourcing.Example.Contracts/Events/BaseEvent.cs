using System;

namespace Orlenko.EventSourcing.Example.Contracts.Events
{
    public class BaseEvent
    {
        public BaseEvent(Guid eventId, DateTime eventDate)
        {
            this.EventDate = eventDate;
            this.EventId = eventId;
        }

        public int Version { get; set; }

        public readonly DateTime EventDate;

        public readonly Guid EventId;
    }
}
