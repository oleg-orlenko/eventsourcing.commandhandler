using Orlenko.EventSourcing.Example.Contracts.Abstractions;
using Orlenko.EventSourcing.Example.Contracts.Models;
using Orlenko.EventSourcing.Example.Domain;
using Orlenko.EventSourcing.Example.Domain.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Repository
{
    public class InlineRestoreGenericCollectionRepository : IGenericCollectionRepository<Item>
    {
        private readonly IEventsStore<BaseEvent<Item>> eventsStore;

        public InlineRestoreGenericCollectionRepository(IEventsStore<BaseEvent<Item>> eventsStore)
        {
            this.eventsStore = eventsStore;
        }

        public async Task<GenericCollection<Item>> GetAsync(CancellationToken cancellationToken = default)
        {
            var allEvents = await this.eventsStore.GetAllAsync(cancellationToken);
            var result = EventSourcedItemsCollection.FromEvents(allEvents);
            return result;
        }

        public async Task PersistAsync(GenericCollection<Item> collection, CancellationToken cancellationToken = default)
        {
            var lastPersistedEventOption = await this.eventsStore.GetLastAsync(cancellationToken);
            var lastPersistedDate = lastPersistedEventOption.Match(DateTime.MinValue, evt => evt.EventDate);

            var changes = collection.GetChangesAfter(lastPersistedDate);

            foreach(var evt in changes)
            {
                await this.eventsStore.AddEventAsync(evt, cancellationToken);
            }
        }
    }
}
