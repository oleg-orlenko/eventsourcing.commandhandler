using Orlenko.EventSourcing.Example.Contracts.Models;
using Orlenko.EventSourcing.Example.Domain;
using Orlenko.EventSourcing.Example.Domain.Events;
using Orlenko.EventSourcing.Example.Repository.MongoDb.Entities;
using System;

namespace Orlenko.EventSourcing.Example.Repository.MongoDb
{
    public static class Converters
    {
        public static BaseEvent<Item> ToBaseItemEvent(this EventStoreEntity e)
        {
            switch (e.Type)
            {
                case nameof(ItemCreatedEvent):
                    var createdItem = new Item(e.ItemId, e.ItemName);
                    return new ItemCreatedEvent(createdItem, e.UserName, e.EventId, e.EventDate);

                case nameof(ItemDeletedEvent):
                    var deletedItem = new Item(e.ItemId, e.ItemName);
                    return new ItemDeletedEvent(deletedItem, e.UserName, e.EventId, e.EventDate);

                case nameof(ItemUpdatedEvent):
                    var updatedItem = new Item(e.ItemId, e.ItemName);
                    return new ItemUpdatedEvent(updatedItem, e.UserName, e.EventId, e.EventDate);

                default:
                    return null;
            }
        }

        public static EventStoreEntity ToEventStoreEntity(this BaseEvent<Item> evt)
        {
            var entity = new EventStoreEntity();
            switch (evt)
            {
                case ItemCreatedEvent created:
                    entity.Type = nameof(ItemCreatedEvent);
                    entity.ItemName = created.Item.Name;
                    break;
                case ItemDeletedEvent deleted:
                    entity.Type = nameof(ItemDeletedEvent);
                    break;
                case ItemUpdatedEvent updated:
                    entity.Type = nameof(ItemUpdatedEvent);
                    entity.ItemName = updated.Item.Name;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Specified type {evt.GetType()} is not supported");
            }

            entity.EventId = evt.EventId;
            entity.EventDate = evt.EventDate;
            entity.ItemId = evt.Item.Id;
            entity.UserName = evt.UserName;

            return entity;
        }
    }
}
