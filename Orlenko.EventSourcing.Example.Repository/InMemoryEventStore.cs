using Orlenko.EventSourcing.Example.Contracts.Abstractions;
using Orlenko.EventSourcing.Example.Domain;
using Orlenko.EventSourcing.Example.Domain.Events;
using Orlenko.EventSourcing.Example.Domain.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Repository
{
    public class InMemoryEventStore : IEventsStore<BaseEvent<Item>>
    {
        private readonly LinkedList<BaseEvent<Item>> events;

        private readonly IEventsPublisher<BaseEvent<Item>> publisher;

        public InMemoryEventStore(IEventsPublisher<BaseEvent<Item>> publisher)
        {
            this.events = new LinkedList<BaseEvent<Item>>();
            this.publisher = publisher;
        }

        public async Task AddEventAsync(BaseEvent<Item> evt, CancellationToken cancellationToken = default)
        {
            this.events.AddLast(evt);
            await this.publisher.PublishAsync(evt, cancellationToken);
        }

        public Task<IEnumerable<BaseEvent<Item>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = new BaseEvent<Item>[events.Count];
            this.events.CopyTo(result, 0);

            return Task.FromResult<IEnumerable<BaseEvent<Item>>>(result);
        }

        public Task<IEnumerable<BaseEvent<Item>>> GetAllForAggregateAsync(Guid aggregateId, CancellationToken cancellationToken = default)
        {
            var result = events.Where(e => e.Item.Id == aggregateId).ToArray();
            return Task.FromResult<IEnumerable<BaseEvent<Item>>>(result);
        }

        public Task<IOption<BaseEvent<Item>>> GetLastAsync(CancellationToken cancellationToken = default)
        {
            if (events.Count == 0)
                return Task.FromResult<IOption<BaseEvent<Item>>>(new None<BaseEvent<Item>>());

            var lastPersisted = this.events.OrderByDescending(x => x.EventDate).First();
            return Task.FromResult<IOption<BaseEvent<Item>>>(new Some<BaseEvent<Item>>(lastPersisted));
        }
    }
}
