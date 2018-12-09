using Orlenko.EventSourcing.Example.Contracts.Abstractions;
using Orlenko.EventSourcing.Example.Contracts.Events;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Core.Aggregates
{
    public class AggregateRoot
    {
        private readonly IAggregateRepository aggregatesRepository;

        public AggregateRoot(IAggregateRepository aggregatesRepository)
        {
            this.aggregatesRepository = aggregatesRepository;
        }

        public async Task<bool> ApplyEventAsync(BaseEvent evt)
        {
            // Lets have a constraint for item Name uniqueness

            switch (evt)
            {
                case ItemCreatedEvent created:
                    var itemExists = await aggregatesRepository.ExistsAsync(created.Name);
                    if (itemExists)
                    {
                        return false;
                    }

                    var newAggregate = new ItemAggregate(created.Id);
                    newAggregate.ApplyEvent(created);
                    await aggregatesRepository.CreateAsync(newAggregate);
                    return true;

                case ItemDeletedEvent deleted:
                    var itemForDeletion = await aggregatesRepository.GetByIdAsync(deleted.Id);
                    if (itemForDeletion == null)
                    {
                        return false;
                    }

                    itemForDeletion.ApplyEvent(deleted);
                    await aggregatesRepository.DeleteAsync(itemForDeletion);
                    return true;

                case ItemUpdatedEvent updated:
                    var itemForUpdate = await aggregatesRepository.GetByIdAsync(updated.Id);
                    if (itemForUpdate == null)
                    {
                        return false;
                    }

                    var itemWithSameName = await aggregatesRepository.ExistsAsync(updated.Name);
                    if (itemWithSameName)
                    {
                        return false;
                    }

                    itemForUpdate.ApplyEvent(updated);
                    return true;

                default:
                    return false;
            }
        }
    }
}
