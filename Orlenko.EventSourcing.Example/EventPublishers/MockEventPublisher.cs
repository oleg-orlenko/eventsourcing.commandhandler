using Orlenko.EventSourcing.Example.Contracts.Abstractions;
using Orlenko.EventSourcing.Example.Domain.Events;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.EventPublishers
{
    public class MockEventPublisher : IEventsPublisher
    {
        public Task PublishAsync(BaseEvent evt)
        {
            return Task.CompletedTask;
        }
    }
}
