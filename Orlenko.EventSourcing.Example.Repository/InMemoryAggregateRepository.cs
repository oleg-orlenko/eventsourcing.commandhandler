using Orlenko.EventSourcing.Example.Contracts.Abstractions;
using Orlenko.EventSourcing.Example.Contracts.Enums;
using Orlenko.EventSourcing.Example.Contracts.Events;
using Orlenko.EventSourcing.Example.Contracts.Models;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Repository
{
    public class InMemoryAggregateRepository : IAggregateRepository<ItemAggregate>
    {
        private readonly ConcurrentDictionary<Guid, ItemAggregate> aggregatesCollection;

        private readonly IEventsStore<BaseItemEvent> eventsStore;

        public InMemoryAggregateRepository(IEventsStore<BaseItemEvent> eventsStore)
        {
            this.aggregatesCollection = new ConcurrentDictionary<Guid, ItemAggregate>();
            this.eventsStore = eventsStore;

            // Some injection of initialized aggregatesCollection might happen here as well, to restore the state of application
        }

        public async Task CommitChangesAsync(ItemAggregate aggregate)
        {
            var stagedEvents = aggregate.GetStagedEvents<BaseItemEvent>();
            foreach(var evt in stagedEvents)
            {
                await this.eventsStore.AddEventAsync(evt);
            }

            switch (aggregate.TransactionalState)
            {
                case AggregateStates.Created:
                case AggregateStates.Updated:
                    this.aggregatesCollection[aggregate.Id] = aggregate;
                    break;

                case AggregateStates.Deleted:
                    this.aggregatesCollection.TryRemove(aggregate.Id, out ItemAggregate agg);
                    break;
            }
        }

        public Task<bool> ExistsAsync(string name)
        {
            var result = false;
            if (!String.IsNullOrEmpty(name))
            {
                result = this.aggregatesCollection.Any(a => a.Value.Name.Equals(name));    
            }

            return Task.FromResult(result);
        }

        public Task<ItemAggregate> GetByIdAsync(Guid id)
        {
            ItemAggregate result = null;
            if (!aggregatesCollection.ContainsKey(id))
            {
                return Task.FromResult(result);
            }

            result = aggregatesCollection[id];
            return Task.FromResult(result);
        }

        public Task RollbackChangesAsync(ItemAggregate aggregate)
        {
            // No idea how to implement this for now
            return Task.CompletedTask;
        }
    }
}
