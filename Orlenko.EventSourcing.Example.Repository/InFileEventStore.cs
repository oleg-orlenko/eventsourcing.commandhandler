using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Orlenko.EventSourcing.Example.Contracts.Abstractions;
using Orlenko.EventSourcing.Example.Domain;
using Orlenko.EventSourcing.Example.Domain.Events;
using Orlenko.EventSourcing.Example.Domain.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Repository
{
    public class InFileEventStore : IEventsStore<BaseEvent<Item>>
    {
        private readonly string fileNameFormat;

        private readonly string pathToEventsStoreFolder;

        private readonly JsonSerializerSettings serializerSettings;

        private readonly IEventsPublisher<BaseEvent<Item>> publisher;

        public InFileEventStore(IHostingEnvironment hostingEnvironment, IEventsPublisher<BaseEvent<Item>> publisher)
        {
            this.publisher = publisher;
            this.fileNameFormat = "eventsStore_{0}.txt";
            this.pathToEventsStoreFolder = Path.Combine(hostingEnvironment.ContentRootPath, "EventsStore");
            if (!Directory.Exists(pathToEventsStoreFolder))
            {
                Directory.CreateDirectory(pathToEventsStoreFolder);
            }

            this.serializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            };
        }

        public async Task AddEventAsync(BaseEvent<Item> evt, CancellationToken cancellationToken = default)
        {
            var filePath = Path.Combine(pathToEventsStoreFolder, String.Format(fileNameFormat, evt.Item.Id));
            using (var streamWriter = new StreamWriter(filePath, true))
            {
                var value = JsonConvert.SerializeObject(evt, this.serializerSettings);
                streamWriter.WriteLine(value);
            }

            await this.publisher.PublishAsync(evt, cancellationToken);
        }

        public Task<IEnumerable<BaseEvent<Item>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var filesList = Directory.GetFiles(pathToEventsStoreFolder);

            var result = new List<BaseEvent<Item>>();
            foreach (var file in filesList)
            {
                var listOfEvents = this.GetEventsFromFile(file);
                result.AddRange(listOfEvents);
            }

            return Task.FromResult<IEnumerable<BaseEvent<Item>>>(result);
        }

        public Task<IEnumerable<BaseEvent<Item>>> GetAllForAggregateAsync(Guid aggregateId, CancellationToken cancellationToken = default)
        {
            var filePath = Path.Combine(pathToEventsStoreFolder, String.Format(fileNameFormat, aggregateId));
            if (!File.Exists(filePath))
            {
                return Task.FromResult<IEnumerable<BaseEvent<Item>>>(Array.Empty<BaseEvent<Item>>());
            }

            var listOfEvents = this.GetEventsFromFile(filePath);
            return Task.FromResult(listOfEvents);
        }

        public Task<IOption<BaseEvent<Item>>> GetLastAsync(CancellationToken cancellationToken = default)
        {
            var lastUpdatedFile = Directory.GetFiles(pathToEventsStoreFolder)
                .Select(x => new { Updated = File.GetLastWriteTimeUtc(x), File = x })
                .OrderByDescending(x => x.Updated)
                .FirstOrDefault()?.File;

            if (lastUpdatedFile is null)
                return Task.FromResult<IOption<BaseEvent<Item>>>(new None<BaseEvent<Item>>());

            IOption<BaseEvent<Item>> result;
            using (var streamReader = new StreamReader(lastUpdatedFile))
            {
                BaseEvent<Item> last = null;
                while (!streamReader.EndOfStream)
                {
                    var serializedEvent = streamReader.ReadLine();
                    last = JsonConvert.DeserializeObject<BaseEvent<Item>>(serializedEvent, this.serializerSettings);
                }

                if (last is null)
                    result = new None<BaseEvent<Item>>();
                else
                    result = new Some<BaseEvent<Item>>(last);
            }

            return Task.FromResult(result);
        }

        private IEnumerable<BaseEvent<Item>> GetEventsFromFile(string fileName)
        {
            var result = new List<BaseEvent<Item>>();
            using (var streamReader = new StreamReader(fileName))
            {
                while (!streamReader.EndOfStream)
                {
                    var serializedEvent = streamReader.ReadLine();
                    var evt = JsonConvert.DeserializeObject<BaseEvent<Item>>(serializedEvent, this.serializerSettings);
                    result.Add(evt);
                }
            }

            return result;
        }
    }
}
