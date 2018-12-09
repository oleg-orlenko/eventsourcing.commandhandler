using Orlenko.EventSourcing.Example.Contracts.Events;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Contracts.Abstractions
{
    public interface IEventsPublisher
    {
        Task PublishAsync(BaseEvent evt);
    }
}
