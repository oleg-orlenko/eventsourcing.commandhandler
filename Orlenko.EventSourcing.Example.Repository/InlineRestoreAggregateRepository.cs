using Orlenko.EventSourcing.Example.Contracts.Abstractions;
using Orlenko.EventSourcing.Example.Contracts.Events;
using Orlenko.EventSourcing.Example.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Repository
{
    public class InlineRestoreAggregateRepository : IAggregateRepository<ItemAggregate>
    {
        private readonly IEventsStore<BaseItemEvent> eventsStore;

        public InlineRestoreAggregateRepository(IEventsStore<BaseItemEvent> eventsStore)
        {
            this.eventsStore = eventsStore;
        }

        public async Task CommitChangesAsync(ItemAggregate aggregate)
        {
            var stagedEvents = aggregate.GetStagedEvents<BaseItemEvent>();
            foreach (var evt in stagedEvents)
            {
                await this.eventsStore.AddEventAsync(evt);
            }

            aggregate.Commit();
        }

        public async Task<IEnumerable<ItemAggregate>> GetByNameAsync(string name)
        {
            var allEvents = await this.eventsStore.GetAllAsync();
            var groupedByAggregateId = allEvents.GroupBy(x => x.ItemId).ToDictionary(x => x.Key, x => x.ToArray());
            var result = new List<ItemAggregate>();
            foreach (var group in groupedByAggregateId)
            {
                var aggregate = this.RestoreAggregateFromEvents(group.Key, group.Value);
                if (aggregate.Name != null && aggregate.Name.Equals(name))
                {
                    result.Add(aggregate);
                }
            }

            return result;
        }

        public async Task<ItemAggregate> GetByIdAsync(Guid id)
        {
            var listOfEventsForAggregate = await this.eventsStore.GetAllForAggregateAsync(id);
            if (listOfEventsForAggregate.Count() == 0)
            {
                return null;
            }

            var result = this.RestoreAggregateFromEvents(id, listOfEventsForAggregate);
            return result;
        }

        private ItemAggregate RestoreAggregateFromEvents(Guid aggregateId, IEnumerable<BaseItemEvent> events)
        {
            var result = new ItemAggregate(aggregateId);
            foreach (var evt in events)
            {
                result.ApplyEvent(evt);
            }
            result.Commit();

            return result;
        }
    }
}
