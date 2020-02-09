using Orlenko.EventSourcing.Example.Domain.Events;
using Orlenko.EventSourcing.Example.Domain.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Contracts.Abstractions
{
    public interface IEventsStore<TEvt> where TEvt : BaseEvent
    {
        Task<IEnumerable<TEvt>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<TEvt>> GetAllForAggregateAsync(Guid aggregateId, CancellationToken cancellationToken = default);

        Task<IOption<TEvt>> GetLastAsync(CancellationToken cancellationToken = default);

        Task AddEventAsync(TEvt evt, CancellationToken cancellationToken = default);
    }
}
