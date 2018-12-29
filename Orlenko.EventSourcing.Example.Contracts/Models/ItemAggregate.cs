using Orlenko.EventSourcing.Example.Contracts.Enums;
using Orlenko.EventSourcing.Example.Contracts.Events;
using System;

namespace Orlenko.EventSourcing.Example.Contracts.Models
{
    public class ItemAggregate : BaseAggregate
    {
        public string Name { get; private set; }

        public AggregateStates TransactionalState { get; private set; }

        public ItemAggregate(Guid id, BaseEvent lastEvent = null) : base(id, lastEvent)
        {
            this.TransactionalState = AggregateStates.NotChanged;
        }

        // For restore from a snapshot
        public ItemAggregate(Guid id, string name, BaseEvent lastEvent) : this(id, lastEvent)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Name = name;
        }

        public override AggregateApplicationResult ApplyEvent(BaseEvent evt)
        {
            if (evt == null)
            {
                throw new ArgumentNullException(nameof(evt));
            }

            switch (evt)
            {
                case ItemCreatedEvent created:
                    Name = created.Name;
                    this.TransactionalState = AggregateStates.Created;
                    break;

                case ItemUpdatedEvent updated:
                    Name = updated.Name;
                    this.TransactionalState = AggregateStates.Updated;
                    break;

                case ItemDeletedEvent deleted:
                    this.TransactionalState = AggregateStates.Deleted;
                    break;
            }

            return base.ApplyEvent(evt);
        }

        public override void Rollback()
        {
            base.Rollback();
            this.TransactionalState = AggregateStates.NotChanged;
        }

        public override void Commit()
        {
            base.Commit();
            this.TransactionalState = AggregateStates.NotChanged;
        }
    }
}
