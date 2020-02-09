using Orlenko.EventSourcing.Example.Contracts.Abstractions;
using Orlenko.EventSourcing.Example.Contracts.Models;
using Orlenko.EventSourcing.Example.Domain.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Repository
{
    public class PerformantInlineRestoreAggregateRepository : IAggregateRepository<ItemAggregate>
    {
        private readonly IEventsStore<BaseItemEvent> eventsStore;

        private readonly ISnapshotsRepository<ItemAggregate> snapshots;

        public PerformantInlineRestoreAggregateRepository(IEventsStore<BaseItemEvent> eventsStore, ISnapshotsRepository<ItemAggregate> snapshots)
        {
            this.eventsStore = eventsStore;
            this.snapshots = snapshots;
        }

        public async Task CommitChangesAsync(ItemAggregate aggregate)
        {
            var stagedEvents = aggregate.GetStagedEvents<BaseItemEvent>();
            foreach (var evt in stagedEvents)
            {
                await eventsStore.AddEventAsync(evt);
            }

            aggregate.Commit();

            await snapshots.SaveAsync(aggregate);
        }

        public async Task<ItemAggregate> GetByIdAsync(Guid id)
        {
            var snapshot = await snapshots.GetAsync(id);
            return snapshot;
        }

        public async Task<IEnumerable<ItemAggregate>> GetByNameAsync(string name)
        {
            var snapshots = await this.snapshots.GetByNameAsync(name);
            return snapshots;
        }
    }
}
