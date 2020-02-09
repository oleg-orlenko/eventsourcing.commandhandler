using Orlenko.EventSourcing.Example.Contracts.Models;
using Orlenko.EventSourcing.Example.Domain.Events;
using Orlenko.EventSourcing.Example.Repository.MongoDb.Entities;
using System;

namespace Orlenko.EventSourcing.Example.Repository.MongoDb
{
    public static class Converters
    {
        public static BaseItemEvent ToBaseItemEvent(this EventStoreEntity e)
        {
            switch (e.Type)
            {
                case nameof(ItemCreatedEvent):
                    return new ItemCreatedEvent(e.ItemId, e.ItemName, e.UserName, e.EventId, e.EventDate, e.Version) as BaseItemEvent;

                case nameof(ItemDeletedEvent):
                    return new ItemDeletedEvent(e.ItemId, e.UserName, e.EventId, e.EventDate, e.Version) as BaseItemEvent;

                case nameof(ItemUpdatedEvent):
                    return new ItemUpdatedEvent(e.ItemId, e.ItemName, e.UserName, e.EventId, e.EventDate, e.Version) as BaseItemEvent;

                default:
                    return null;
            }
        }

        public static EventStoreEntity ToEventStoreEntity(this BaseItemEvent evt)
        {
            var entity = new EventStoreEntity();
            switch (evt)
            {
                case ItemCreatedEvent created:
                    entity.Type = nameof(ItemCreatedEvent);
                    entity.ItemName = created.Name;
                    break;
                case ItemDeletedEvent deleted:
                    entity.Type = nameof(ItemDeletedEvent);
                    break;
                case ItemUpdatedEvent updated:
                    entity.Type = nameof(ItemUpdatedEvent);
                    entity.ItemName = updated.Name;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Specified type {evt.GetType()} is not supported");
            }

            entity.EventId = evt.EventId;
            entity.EventDate = evt.EventDate;
            entity.ItemId = evt.ItemId;
            entity.UserName = evt.UserName;
            entity.Version = evt.Version;

            return entity;
        }

        public static ItemAggregate ToItemAggregate(this ItemAggregateSnapshotEntity e)
        {
            var lastEvent = e.LastEvent?.ToBaseItemEvent();
            var result = new ItemAggregate(e.ItemId, e.Name, lastEvent);
            return result;
        }

        public static ItemAggregateSnapshotEntity ToSnapshotEntity(this ItemAggregate aggregate)
        {
            var lastEvent = aggregate.LastEvent as BaseItemEvent;
            var result = new ItemAggregateSnapshotEntity
            {
                ItemId = aggregate.Id,
                LastEvent = lastEvent?.ToEventStoreEntity(),
                Name = aggregate.Name
            };

            return result;
        }
    }
}
