using System;
using System.Threading.Tasks;
using Orlenko.EventSourcing.Example.Contracts.Abstractions;
using Orlenko.EventSourcing.Example.Contracts.Commands;
using Microsoft.Extensions.Logging;
using Orlenko.EventSourcing.Example.Domain;
using System.Threading;

namespace Orlenko.EventSourcing.Example.Core.CommandHandlers
{
    public class ItemsCommandHandler : ICommandHandler
    {
        private readonly IGenericCollectionRepository<Item> repository;

        private readonly ILogger<ItemsCommandHandler> logger;

        public ItemsCommandHandler(IGenericCollectionRepository<Item> repository, ILogger<ItemsCommandHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        /// <summary>
        /// Handles all types of command asynchronously.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">If command is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">If command has unsupported type</exception>
        /// <exception cref="Exception">In case of more generic exception</exception>
        public async Task HandleAsync(BaseItemCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));
            
            try
            {
                GenericCollection<Item> collection = null;
                bool mutated = false;
                switch (command)
                {
                    case CreateItemCommand create:
                        collection = await this.repository.GetAsync(cancellationToken);
                        mutated = collection.WithSecurityScope(create.UserName).Add(create.Item);
                        break;

                    case DeleteItemCommand delete:
                        collection = await this.repository.GetAsync(cancellationToken);
                        mutated = collection.GetById(delete.ItemId).Match(false, item => collection.WithSecurityScope(delete.UserName).Remove(item));
                        break;

                    case UpdateItemCommand update:
                        collection = await this.repository.GetAsync(cancellationToken);
                        mutated = collection.WithSecurityScope(update.UserName).Update(update.Item);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(command));
                }

                if (mutated)
                {
                    await this.repository.PersistAsync(collection);
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
