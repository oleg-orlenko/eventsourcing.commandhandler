using Orlenko.EventSourcing.Example.Domain.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Contracts.Abstractions
{
    public interface IEventsPublisher<TEvt> where TEvt : BaseEvent
    {
        Task PublishAsync(TEvt evt, CancellationToken cancellationToken = default);
    }
}
