﻿using Orlenko.EventSourcing.Example.Contracts.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Orlenko.EventSourcing.Example.Contracts.Models
{
    public abstract class BaseAggregate
    {
        public BaseEvent LastEvent { get; private set; }

        public readonly Guid Id;

        protected readonly Queue<BaseEvent> StagedEvents;

        public BaseAggregate(Guid id)
        {
            this.Id = id;
            this.StagedEvents = new Queue<BaseEvent>();
            this.LastEvent = null;
        }

        private int stagedVersion;

        public virtual AggregateApplicationResult ApplyEvent(BaseEvent evt)
        {
            if (evt == null)
            {
                throw new ArgumentNullException(nameof(evt));    
            }

            // I have concerns here, but how the handler would now the version before event appliation ?
            if (evt.Version == 0) // For now this is the definition of a new event
            {
                evt.Version = ++this.stagedVersion;
                this.StagedEvents.Enqueue(evt);
                this.LastEvent = evt;
            }
            
            // This trick might help, but commit of aggregate's staged events is happenning inside AggregateRoot and not in CommandHandler
            // I dont like the idea of getting aggregate version from AggregateRoot
            // Anyway - TBD
            return new SuccessAggregateApplicationResult(); 
        }

        public virtual void Commit()
        {
            this.StagedEvents.Clear();
        }

        public virtual void Rollback()
        {
            // TODO: define what to do here
        }

        public virtual IEnumerable<TEvt> GetStagedEvents<TEvt>() where TEvt : BaseEvent
        {
            var result = this.StagedEvents.OfType<TEvt>().ToArray();
            return result;
        }
    }
}
