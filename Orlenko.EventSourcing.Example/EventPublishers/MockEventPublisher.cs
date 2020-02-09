using Orlenko.EventSourcing.Example.Contracts.Abstractions;
using Orlenko.EventSourcing.Example.Domain;
using Orlenko.EventSourcing.Example.Domain.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.EventPublishers
{
    public class MockEventPublisher : IEventsPublisher<BaseEvent<Item>>
    {
        public Task PublishAsync(BaseEvent<Item> evt, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
