using System;
using System.Threading.Tasks;
using Orlenko.EventSourcing.Example.Contracts.Abstractions;
using Orlenko.EventSourcing.Example.Contracts.Commands;
using Orlenko.EventSourcing.Example.Contracts.Events;
using Orlenko.EventSourcing.Example.Core.Aggregates;
using Microsoft.Extensions.Logging;

namespace Orlenko.EventSourcing.Example.Core.CommandHandlers
{
    public class DefaultCommandHandler : ICommandHandler
    {
        private readonly AggregateRoot root;

        private readonly IEventsStore eventStore;

        private readonly IEventsPublisher publisher;

        private readonly ILogger<DefaultCommandHandler> logger;

        public DefaultCommandHandler(AggregateRoot root, IEventsStore eventStore, IEventsPublisher publisher, ILogger<DefaultCommandHandler> logger)
        {
            this.root = root;
            this.eventStore = eventStore;
            this.publisher = publisher;
        }

        public async Task HandleAsync(BaseItemCommand command)
        {
            try
            {
                if (command == null)
                {
                    throw new ArgumentNullException(nameof(command));
                }

                BaseItemEvent evt;
                switch (command)
                {
                    case CreateItemCommand create:
                        evt = new ItemCreatedEvent(command.Item.Id, command.Item.Name, command.UserName, DateTime.UtcNow);
                        break;
                    case DeleteItemCommand delete:
                        evt = new ItemDeletedEvent(delete.Item.Id, delete.UserName, DateTime.UtcNow);
                        break;
                    case UpdateItemCommand update:
                        evt = new ItemUpdatedEvent(update.Item.Id, update.Item.Name, update.UserName, DateTime.UtcNow);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(command));
                }

                var successfullApply = await this.root.ApplyEventAsync(evt);
                if (successfullApply)
                {
                    await this.eventStore.AddEventAsync(evt);
                    await this.publisher.PublishAsync(evt);
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
