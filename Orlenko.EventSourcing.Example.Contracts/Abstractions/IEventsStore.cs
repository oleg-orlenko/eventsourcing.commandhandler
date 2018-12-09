using Orlenko.EventSourcing.Example.Contracts.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Contracts.Abstractions
{
    public interface IEventsStore
    {
        Task<IEnumerable<BaseEvent>> GetAllAsync();

        Task<IEnumerable<BaseEvent>> GetAllForAggregateAsync(Guid aggregateId);

        Task AddEventAsync(BaseEvent evt);
    }
}
