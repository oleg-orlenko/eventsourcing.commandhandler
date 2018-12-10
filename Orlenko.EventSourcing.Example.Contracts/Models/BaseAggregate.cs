using Orlenko.EventSourcing.Example.Contracts.Events;
using System;
using System.Collections.Generic;

namespace Orlenko.EventSourcing.Example.Contracts.Models
{
    public class BaseAggregate
    {
        public readonly Guid Id;

        private readonly LinkedList<BaseEvent> appliedEvents;

        public BaseAggregate(Guid id)
        {
            this.Id = id;
            this.appliedEvents = new LinkedList<BaseEvent>();
        }

        public int Version { get; private set; }

        public DateTime LastChanged { get; private set; }

        public virtual AggregateApplicationResult ApplyEvent(BaseEvent evt)
        {
            if (evt == null)
            {
                throw new ArgumentNullException(nameof(evt));    
            }

            this.Version++;
            this.LastChanged = evt.EventDate;
            this.appliedEvents.AddLast(evt);

            // I have concerns here, but how the handler would now the version before event appliation ?
            evt.Version = this.Version;
            // This trick might help, but commit of aggregate's staged events is happenning inside AggregateRoot and not in CommandHandler
            // I dont like the idea of getting aggregate version from AggregateRoot
            // Anyway - TBD
            return new SuccessAggregateApplicationResult(this.Version); 
        }
    }
}
