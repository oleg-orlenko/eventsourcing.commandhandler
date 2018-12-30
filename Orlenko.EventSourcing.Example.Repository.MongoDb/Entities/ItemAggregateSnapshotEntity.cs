using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Orlenko.EventSourcing.Example.Repository.MongoDb.Entities
{
    public class ItemAggregateSnapshotEntity
    {
        [BsonId]
        public Guid ItemId { get; set; }

        public string Name { get; set; }

        public EventStoreEntity LastEvent { get; set; }
    }
}
