using Orlenko.EventSourcing.Example.Domain;
using Orlenko.EventSourcing.Example.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Orlenko.EventSourcing.Example.Contracts.Models
{
    public class EventSourcedItemsCollection : ItemsList
    {
        private readonly LinkedList<BaseEvent<Item>> appliedEvents;

        public int Version { get; private set; }

        public DateTime Updated { get; private set; }

        public EventSourcedItemsCollection(IEnumerable<Item> items) : base(items)
        {
            appliedEvents = new LinkedList<BaseEvent<Item>>();
        }

        public override bool Add(Item item)
        {
            var evt = new ItemCreatedEvent(item, "unknown", Guid.NewGuid(), DateTime.UtcNow);
            return ApplyEvent(evt);
        }

        public override bool Remove(Item item)
        {
            var evt = new ItemDeletedEvent(item, "unknown", Guid.NewGuid(), DateTime.UtcNow);
            return ApplyEvent(evt);
        }

        public override bool Update(Item newState)
        {
            var evt = new ItemUpdatedEvent(newState, "unknown", Guid.NewGuid(), DateTime.UtcNow);
            return ApplyEvent(evt);
        }

        private bool ApplyEvent(BaseEvent<Item> evt)
        {
            bool applied;
            switch (evt)
            {
                case ItemCreatedEvent created:
                    applied = base.Add(created.Item);
                    break;

                case ItemUpdatedEvent updated:
                    applied = base.Update(updated.Item);
                    break;

                case ItemDeletedEvent deleted:
                    applied = base.Remove(deleted.Item);
                    break;

                default:
                    return false;
            }

            if (applied)
            {
                appliedEvents.AddLast(evt);
                Version++;
                Updated = evt.EventDate;
            }

            return applied;
        }

        public override IEnumerable<BaseEvent<Item>> GetChangesAfter(DateTime date)
        {
            var result = appliedEvents.Where(x => x.EventDate > date).ToArray();
            return result;
        }

        public static EventSourcedItemsCollection FromEvents(IEnumerable<BaseEvent<Item>> events)
        {
            var result = new EventSourcedItemsCollection(Array.Empty<Item>());

            foreach (var evt in events)
            {
                result.ApplyEvent(evt);
            }

            return result;
        }
    }
}
