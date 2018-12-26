using Orlenko.EventSourcing.Example.Contracts.Abstractions;
using Orlenko.EventSourcing.Example.Contracts.Events;
using Orlenko.EventSourcing.Example.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Core.Aggregates
{
    public class AggregateRoot
    {
        private readonly IAggregateRepository<ItemAggregate> aggregatesRepository;

        private readonly List<ItemAggregate> stagedAggregates;

        public AggregateRoot(IAggregateRepository<ItemAggregate> aggregatesRepository)
        {
            this.aggregatesRepository = aggregatesRepository;
            this.stagedAggregates = new List<ItemAggregate>();
        }

        public async Task<AggregateApplicationResult> ApplyEventAsync(BaseEvent evt)
        {
            // Lets have a constraint for item Name uniqueness
            ItemAggregate aggregate;
            BaseItemEvent itemEvent;
            switch (evt)
            {
                case ItemCreatedEvent created:
                    var itemsWithSameName = await aggregatesRepository.GetByNameAsync(created.Name);
                    if (itemsWithSameName.Any(x => x.LastEvent != null && !(x.LastEvent is ItemDeletedEvent)))
                    {
                        return new ItemAlreadyExistsApplicationResult("Item with the same name already exists");
                    }

                    aggregate = new ItemAggregate(created.ItemId);
                    itemEvent = created;
                    break;

                case ItemDeletedEvent deleted:
                    aggregate = await aggregatesRepository.GetByIdAsync(deleted.ItemId);
                    if (aggregate == null || aggregate.LastEvent == null)
                    {
                        return new ItemNotFoundApplicationResult();
                    }

                    if (aggregate.LastEvent is ItemDeletedEvent)
                    {
                        // It is already deleted
                        return new SuccessAggregateApplicationResult();
                    }

                    itemEvent = deleted;
                    break;

                case ItemUpdatedEvent updated:
                    aggregate = await aggregatesRepository.GetByIdAsync(updated.ItemId);
                    if (aggregate == null || aggregate.LastEvent == null || aggregate.LastEvent is ItemDeletedEvent)
                    {
                        return new ItemNotFoundApplicationResult();
                    }

                    if (aggregate.LastEvent is ItemUpdatedEvent lastUpdated && lastUpdated.Name.Equals(updated.Name))
                    {
                        // This aggregate has already got this name
                        return new SuccessAggregateApplicationResult();
                    }

                    // Looking for another aggregate with this name
                    var itemsWithSameNameForUpdate = await aggregatesRepository.GetByNameAsync(updated.Name);
                    if (itemsWithSameNameForUpdate.Any(x => x.LastEvent != null && !(x.LastEvent is ItemDeletedEvent)))
                    {
                        return new ItemAlreadyExistsApplicationResult("Item with the same name already exists");
                    }

                    itemEvent = updated;
                    break;

                default:
                    return new FailedAggregateApplicationResult($"Specified event type {evt.GetType().Name} is not supported");
            }

            var applicationResult = aggregate.ApplyEvent(itemEvent);
            switch (applicationResult)
            {
                case SuccessAggregateApplicationResult success:
                    // This method might be a root specific, because of several aggregate repositories might be impacted by one event
                    this.stagedAggregates.Add(aggregate);
                    break;
            }

            return applicationResult;
        }

        public async Task CommitAsync()
        {
            foreach(var aggregate in this.stagedAggregates)
            {
                // Persists aggregate state
                await aggregatesRepository.CommitChangesAsync(aggregate);

                // Sets applies all staged events to aggregate state
                aggregate.Commit();
            }

            // On purpose I am not clearing staged aggregates collection untill all of them are commited 
            // (Rollback might be triggered in case of error)
            this.stagedAggregates.Clear();
        }

        public async Task RollbackAsync()
        {
            // How to handle exception here ?(
            foreach(var aggregate in this.stagedAggregates)
            {
                await aggregatesRepository.RollbackChangesAsync(aggregate);    
                aggregate.Rollback();
            }

            this.stagedAggregates.Clear();
        }
    }
}
