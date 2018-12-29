using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Orlenko.EventSourcing.Example.Repository.MongoDb.Entities
{
    public class EventStoreEntity
    {
        [BsonId]
        public Guid EventId { get; set; }

        public string Type { get; set; }

        public int Version { get; set; }

        public string UserName { get; set; }
        
        public Guid ItemId { get; set; }

        public DateTime EventDate { get; set; }

        public string ItemName { get; set; }
    }
}
