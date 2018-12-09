using Orlenko.EventSourcing.Example.Contracts.Events;
using System;
using System.Collections.Generic;

namespace Orlenko.EventSourcing.Example.Contracts.Models
{
    public class BaseAggregate
    {
        public readonly Guid Id;

        protected readonly LinkedList<BaseEvent> appliedEvents;

        public BaseAggregate(Guid id)
        {
            this.Id = id;
            this.appliedEvents = new LinkedList<BaseEvent>();
        }

        public int Version { get; private set; }

        public DateTime LastChanged { get; private set; }

        public virtual bool ApplyEvent(BaseEvent evt)
        {
            if (evt == null)
            {
                throw new ArgumentNullException(nameof(evt));    
            }

            this.Version++;
            this.LastChanged = evt.EventDate;
            this.appliedEvents.AddLast(evt);

            evt.Version = this.Version;
            return true;
        }
    }
}
