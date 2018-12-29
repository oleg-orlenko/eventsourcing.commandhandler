using Microsoft.Extensions.Configuration;
using System;

namespace Orlenko.EventSourcing.Example.Repository.MongoDb.Configuration
{
    public class MongoSnapshotsConfig : MongoBaseConfig
    {
        public readonly string SnapshotsCollection;

        public MongoSnapshotsConfig(IConfigurationSection config) : base(config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            this.SnapshotsCollection = config["snapshotsCollection"] ?? throw new Exception("Missing configuration value for eventsCollection");
        }

        public MongoSnapshotsConfig(string snapshotsCollection, string serverConnection, string databaseName) : base(serverConnection, databaseName)
        {
            if (string.IsNullOrEmpty(snapshotsCollection))
            {
                throw new ArgumentNullException(nameof(snapshotsCollection));

            }
            this.SnapshotsCollection = snapshotsCollection;
        }
    }
}
