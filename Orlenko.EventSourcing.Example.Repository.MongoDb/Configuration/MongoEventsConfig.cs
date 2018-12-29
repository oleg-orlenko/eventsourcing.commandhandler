using Microsoft.Extensions.Configuration;
using System;

namespace Orlenko.EventSourcing.Example.Repository.MongoDb.Configuration
{
    public class MongoEventsConfig : MongoBaseConfig
    {
        public readonly string EventsCollection;

        public MongoEventsConfig(IConfigurationSection config) : base(config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            this.EventsCollection = config["eventsCollection"] ?? throw new Exception("Missing configuration value for eventsCollection");
        }

        public MongoEventsConfig(string eventsCollection, string serverConnection, string databaseName) : base(serverConnection, databaseName)
        {
            if (string.IsNullOrEmpty(eventsCollection))
            {
                throw new ArgumentNullException(nameof(serverConnection));
            }

            this.EventsCollection = eventsCollection;
        }
    }
}
