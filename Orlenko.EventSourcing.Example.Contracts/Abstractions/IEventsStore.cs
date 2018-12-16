using Orlenko.EventSourcing.Example.Contracts.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Contracts.Abstractions
{
    public interface IEventsStore<TEvt> where TEvt : BaseEvent
    {
        Task<IEnumerable<TEvt>> GetAllAsync();

        Task<IEnumerable<TEvt>> GetAllForAggregateAsync(Guid aggregateId);

        Task AddEventAsync(TEvt evt);
    }
}
