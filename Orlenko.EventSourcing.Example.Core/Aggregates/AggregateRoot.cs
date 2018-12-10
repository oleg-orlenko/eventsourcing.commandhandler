using Orlenko.EventSourcing.Example.Contracts.Abstractions;
using Orlenko.EventSourcing.Example.Contracts.Events;
using Orlenko.EventSourcing.Example.Contracts.Models;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Core.Aggregates
{
    public class AggregateRoot
    {
        private readonly IAggregateRepository<ItemAggregate> aggregatesRepository;

        public AggregateRoot(IAggregateRepository<ItemAggregate> aggregatesRepository)
        {
            this.aggregatesRepository = aggregatesRepository;
        }

        public async Task<AggregateApplicationResult> ApplyEventAsync(BaseEvent evt)
        {
            // Lets have a constraint for item Name uniqueness
            ItemAggregate aggregate;
            switch (evt)
            {
                case ItemCreatedEvent created:
                    var itemExists = await aggregatesRepository.ExistsAsync(created.Name);
                    if (itemExists)
                    {
                        return new FailedAggregateApplicationResult("Item with the same name already exists.");
                    }

                    aggregate = new ItemAggregate(created.Id);
                    break;

                case ItemDeletedEvent deleted:
                    aggregate = await aggregatesRepository.GetByIdAsync(deleted.Id);
                    if (aggregate == null)
                    {
                        return new FailedAggregateApplicationResult("Specified item was not found.");
                    }
                    
                    break;

                case ItemUpdatedEvent updated:
                    aggregate = await aggregatesRepository.GetByIdAsync(updated.Id);
                    if (aggregate == null)
                    {
                        return new FailedAggregateApplicationResult("Specified item was not found.");
                    }

                    var itemWithSameNameExists = await aggregatesRepository.ExistsAsync(updated.Name);
                    if (itemWithSameNameExists)
                    {
                        return new FailedAggregateApplicationResult("Item with the same name already exists.");
                    }
                    
                    break;

                default:
                    return new FailedAggregateApplicationResult($"Specified event type {evt.GetType().Name} is not supported");
            }

            var applicationResult = aggregate.ApplyEvent(evt);
            switch(applicationResult)
            {
                case SuccessAggregateApplicationResult success:
                    // This method might be a root specific, because of several aggregate repositories might be impacted by one event
                    await aggregatesRepository.CommitChangesAsync(aggregate);
                    break;
            }
            
            return applicationResult;
        }
    }
}
