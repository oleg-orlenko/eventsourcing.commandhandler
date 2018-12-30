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
            var entity = evt.ToEventStoreEntity();
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
            var result = dbEntities.Select(e => e.ToBaseItemEvent()).Where(e => e != null).ToArray();
            return result;
        }
    }
}
