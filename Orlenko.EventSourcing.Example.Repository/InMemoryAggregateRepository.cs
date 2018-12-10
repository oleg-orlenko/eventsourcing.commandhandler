using Orlenko.EventSourcing.Example.Contracts.Abstractions;
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

        private readonly IEventsStore eventsStore;

        public InMemoryAggregateRepository(IEventsStore eventsStore)
        {
            this.aggregatesCollection = new ConcurrentDictionary<Guid, ItemAggregate>();
            this.eventsStore = eventsStore;

            // Some injection of initialized aggregatesCollection might happen here as well, to restore the state of application
        }

        public Task CommitChangesAsync(ItemAggregate aggregate)
        {
            throw new NotImplementedException();
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
    }
}
