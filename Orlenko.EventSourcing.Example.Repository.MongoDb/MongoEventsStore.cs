using MongoDB.Driver;
using Orlenko.EventSourcing.Example.Contracts.Abstractions;
using Orlenko.EventSourcing.Example.Domain;
using Orlenko.EventSourcing.Example.Domain.Events;
using Orlenko.EventSourcing.Example.Domain.Options;
using Orlenko.EventSourcing.Example.Repository.MongoDb.Configuration;
using Orlenko.EventSourcing.Example.Repository.MongoDb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Repository.MongoDb
{
    public class MongoEventsStore : IEventsStore<BaseEvent<Item>>
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

        public async Task AddEventAsync(BaseEvent<Item> evt, CancellationToken cancellationToken = default)
        {
            var entity = evt.ToEventStoreEntity();
            await collection.InsertOneAsync(entity);
        }

        public async Task<IEnumerable<BaseEvent<Item>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var filter = Builders<EventStoreEntity>.Filter.Empty;
            var cursor = await collection.FindAsync(filter);
            var dbEntities = await cursor.ToListAsync();
            var result = ConvertEntitiesList(dbEntities);
            return result;
        }

        public async Task<IEnumerable<BaseEvent<Item>>> GetAllForAggregateAsync(Guid aggregateId, CancellationToken cancellationToken = default)
        {
            var filter = Builders<EventStoreEntity>.Filter.Eq(x => x.ItemId, aggregateId);
            var cursor = await collection.FindAsync(filter);
            var dbEntities = await cursor.ToListAsync();
            var result = ConvertEntitiesList(dbEntities);
            return result;
        }

        public async Task<IOption<BaseEvent<Item>>> GetLastAsync(CancellationToken cancellationToken = default)
        {
            var filter = Builders<EventStoreEntity>.Filter.Empty;
            var list = await collection.Find(filter).Sort("{EventDate: -1}").Limit(1).ToListAsync();
            if (list.Count == 0)
                return new None<BaseEvent<Item>>();

            var result = list[0].ToBaseItemEvent();
            return new Some<BaseEvent<Item>>(result);
        }

        private BaseEvent<Item>[] ConvertEntitiesList(IEnumerable<EventStoreEntity> dbEntities)
        {
            var result = dbEntities.Select(e => e.ToBaseItemEvent()).Where(e => e != null).ToArray();
            return result;
        }
    }
}
