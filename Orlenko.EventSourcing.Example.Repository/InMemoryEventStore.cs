using Orlenko.EventSourcing.Example.Contracts.Abstractions;
using Orlenko.EventSourcing.Example.Contracts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Repository
{
    public class InMemoryEventStore : IEventsStore<BaseItemEvent>
    {
        private readonly LinkedList<BaseItemEvent> events;

        public InMemoryEventStore()
        {
            events = new LinkedList<BaseItemEvent>();
        }

        public Task AddEventAsync(BaseItemEvent evt)
        {
            events.AddLast(evt);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<BaseItemEvent>> GetAllAsync()
        {
            var result = new BaseItemEvent[events.Count];
            events.CopyTo(result, 0);

            return Task.FromResult<IEnumerable<BaseItemEvent>>(result);
        }

        public Task<IEnumerable<BaseItemEvent>> GetAllForAggregateAsync(Guid aggregateId)
        {
            var result = events.Where(e => e.ItemId == aggregateId).ToArray();
            return Task.FromResult<IEnumerable<BaseItemEvent>>(result);
        }
    }
}
