using MongoDB.Driver;
using Orlenko.EventSourcing.Example.Contracts.Abstractions;
using Orlenko.EventSourcing.Example.Contracts.Events;
using Orlenko.EventSourcing.Example.Contracts.Models;
using Orlenko.EventSourcing.Example.Repository.MongoDb.Configuration;
using Orlenko.EventSourcing.Example.Repository.MongoDb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Repository.MongoDb
{
    public class MongoSnapshotsRepository : ISnapshotsRepository<ItemAggregate>
    {
        private readonly MongoClient client;

        private readonly IMongoDatabase database;

        private readonly IMongoCollection<ItemAggregateSnapshotEntity> collection;

        public MongoSnapshotsRepository(MongoSnapshotsConfig config)
        {
            client = new MongoClient(config.ServerConnection);
            database = client.GetDatabase(config.DatabaseName);
            collection = database.GetCollection<ItemAggregateSnapshotEntity>(config.SnapshotsCollection);

        }
        public async Task<ItemAggregate> GetAsync(Guid id)
        {
            var filter = Builders<ItemAggregateSnapshotEntity>.Filter.Eq(x => x.ItemId, id);
            var cursor = await collection.FindAsync(filter);
            var dbEntity = cursor.FirstOrDefault();
            if (dbEntity == null)
            {
                return null;
            }

            var result = dbEntity.ToItemAggregate();
            return result;
        }

        public async Task<IEnumerable<ItemAggregate>> GetByNameAsync(string name)
        {
            var filter = Builders<ItemAggregateSnapshotEntity>.Filter.Eq(x => x.Name, name);
            var cursor = await collection.FindAsync(filter);
            var dbEntities = await cursor.ToListAsync();

            var result = dbEntities.Select(i => i.ToItemAggregate());
            return result;
        }

        public Task SaveAsync(ItemAggregate aggregate)
        {
            if (aggregate.LastEvent == null)
            {
                return Task.CompletedTask;
            }

            var dbEntity = aggregate.ToSnapshotEntity();

            var filter = Builders<ItemAggregateSnapshotEntity>.Filter.Eq(x => x.ItemId, aggregate.Id);

            switch (aggregate.LastEvent)
            {
                case ItemCreatedEvent created:
                    return this.collection.InsertOneAsync(dbEntity);

                case ItemUpdatedEvent updated:
                    return this.collection.ReplaceOneAsync(filter, dbEntity);

                case ItemDeletedEvent deleted:
                    return this.collection.DeleteOneAsync(filter);

                default:
                    throw new ArgumentOutOfRangeException($"Specified type {aggregate.LastEvent.GetType()} is not supported");
            }
        }
    }
}
