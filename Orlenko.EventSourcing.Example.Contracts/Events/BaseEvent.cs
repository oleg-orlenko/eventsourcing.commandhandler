using System;

namespace Orlenko.EventSourcing.Example.Contracts.Events
{
    public class BaseEvent
    {
        public BaseEvent(Guid id, DateTime eventDate)
        {
            EventDate = eventDate;
            Id = id;
        }

        public int Version { get; set; }

        public readonly DateTime EventDate;

        public readonly Guid Id;
    }
}
