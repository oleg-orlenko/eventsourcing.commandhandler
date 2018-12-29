using System;

namespace Orlenko.EventSourcing.Example.Contracts.Events
{
    public abstract class BaseEvent
    {
        protected BaseEvent(Guid eventId, DateTime eventDate, int version)
        {
            this.EventDate = eventDate;
            this.EventId = eventId;
            this.Version = version;
        }

        public int Version { get; set; }

        public readonly DateTime EventDate;

        public readonly Guid EventId;
    }
}
