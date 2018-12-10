using System;
using System.Threading.Tasks;
using Orlenko.EventSourcing.Example.Contracts.Abstractions;
using Orlenko.EventSourcing.Example.Contracts.Commands;
using Orlenko.EventSourcing.Example.Contracts.Events;
using Orlenko.EventSourcing.Example.Core.Aggregates;
using Microsoft.Extensions.Logging;
using Orlenko.EventSourcing.Example.Contracts.Models;

namespace Orlenko.EventSourcing.Example.Core.CommandHandlers
{
    public class DefaultCommandHandler : ICommandHandler
    {
        private readonly AggregateRoot root;

        private readonly IEventsPublisher publisher;

        private readonly ILogger<DefaultCommandHandler> logger;

        public DefaultCommandHandler(AggregateRoot root, IEventsPublisher publisher, ILogger<DefaultCommandHandler> logger)
        {
            this.root = root;
            this.publisher = publisher;
        }

        /// <summary>
        /// Handles all types of command asynchronously.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">If command is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">If command has unsupported type</exception>
        /// <exception cref="Exception">In case of more generic exception</exception>
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

                var applicationResult = await this.root.ApplyEventAsync(evt);
                switch(applicationResult)
                {
                    case SuccessAggregateApplicationResult success:
                        // Commit staged events might happen here
                        // await this.root.CommitAsync();
                        // Success result might have a list of Staged events in more complex implementation
                        await this.publisher.PublishAsync(evt);
                        break;

                    case FailedAggregateApplicationResult failed:
                        throw new Exception(failed.Error);
                    
                    default:
                        throw new Exception($"Unsupported aggregate application result type {applicationResult.GetType().Name}");
                }
            }
            catch (Exception e)
            {
                this.logger.LogError(e, $"Failed to process command {command.GetType().Name}");
                throw;
            }
        }
    }
}
