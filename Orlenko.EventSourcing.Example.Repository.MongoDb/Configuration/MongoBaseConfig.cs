using Microsoft.Extensions.Configuration;
using System;

namespace Orlenko.EventSourcing.Example.Repository.MongoDb
{
    public class MongoBaseConfig
    {
        public readonly string ServerConnection;

        public readonly string DatabaseName;

        public MongoBaseConfig(IConfigurationSection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            this.ServerConnection = config["serverConnection"] ?? throw new Exception("Missing configuration value for serverConnection");
            this.DatabaseName = config["databaseName"] ?? throw new Exception("Missing configuration value for databaseName");
        }

        public MongoBaseConfig(string serverConnection, string databaseName)
        {
            if (string.IsNullOrEmpty(serverConnection))
            {
                throw new ArgumentNullException(nameof(serverConnection));
            }

            this.ServerConnection = serverConnection;

            if (string.IsNullOrEmpty(databaseName))
            {
                throw new ArgumentNullException(nameof(databaseName));
            }

            this.DatabaseName = databaseName;
        }
    }
}
