using MongoDB.Driver;
using Orlenko.EventSourcing.Example.Contracts.Abstractions;
using Orlenko.EventSourcing.Example.Contracts.Events;
using Orlenko.EventSourcing.Example.Repository.MongoDb.Configuration;
using Orlenko.EventSourcing.Example.Repository.MongoDb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Repository.MongoDb
{
    public class MongoEventsStore : IEventsStore<BaseItemEvent>
    {
        private readonly MongoClient client;

        private readonly IMongoDatabase database;

        private readonly IMongoCollection<EventStoreEntity> collection;

        public MongoEventsStore(MongoEventsConfig config)
        {
            client = new MongoClient(config.ServerConnection);
            database = client.GetDatabase(config.DatabaseName);
            collection = database.GetCollection<EventStoreEntity>(config.EventsCollection);
        }

        public async Task AddEventAsync(BaseItemEvent evt)
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

            await collection.InsertOneAsync(entity);
        }

        public async Task<IEnumerable<BaseItemEvent>> GetAllAsync()
        {
            var filter = Builders<EventStoreEntity>.Filter.Empty;
            var cursor = await collection.FindAsync(filter);
            var dbEntities = await cursor.ToListAsync();
            var result = ConvertEntitiesList(dbEntities);
            return result;
        }

        public async Task<IEnumerable<BaseItemEvent>> GetAllForAggregateAsync(Guid aggregateId)
        {
            var filter = Builders<EventStoreEntity>.Filter.Eq(x => x.ItemId, aggregateId);
            var cursor = await collection.FindAsync(filter);
            var dbEntities = await cursor.ToListAsync();
            var result = ConvertEntitiesList(dbEntities);
            return result;
        }

        private BaseItemEvent[] ConvertEntitiesList(IEnumerable<EventStoreEntity> dbEntities)
        {
            var result = dbEntities.Select(e =>
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
            }).Where(e => e != null).ToArray();

            return result;
        }
    }
}
