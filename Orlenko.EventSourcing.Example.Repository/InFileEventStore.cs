using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Orlenko.EventSourcing.Example.Contracts.Abstractions;
using Orlenko.EventSourcing.Example.Contracts.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Repository
{
    public class InFileEventStore : IEventsStore<BaseItemEvent>
    {
        private readonly string fileNameFormat;

        private readonly string pathToEventsStoreFolder;

        private readonly JsonSerializerSettings serializerSettings;

        public InFileEventStore(IHostingEnvironment hostingEnvironment)
        {
            fileNameFormat = "eventsStore_{0}.txt";
            pathToEventsStoreFolder = Path.Combine(hostingEnvironment.ContentRootPath, "EventsStore");
            if (!Directory.Exists(pathToEventsStoreFolder))
            {
                Directory.CreateDirectory(pathToEventsStoreFolder);
            }

            this.serializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            };
        }

        public Task AddEventAsync(BaseItemEvent evt)
        {
            var filePath = Path.Combine(pathToEventsStoreFolder, String.Format(fileNameFormat, evt.ItemId));
            using (var streamWriter = new StreamWriter(filePath, true))
            {
                var value = JsonConvert.SerializeObject(evt, this.serializerSettings);
                streamWriter.WriteLine(value);
            }

            return Task.CompletedTask;
        }

        public Task<IEnumerable<BaseItemEvent>> GetAllAsync()
        {
            var directoryName = Path.GetDirectoryName(fileNameFormat);
            var filesList = Directory.GetFiles(pathToEventsStoreFolder);

            var result = new List<BaseItemEvent>();
            foreach (var file in filesList)
            {
                var listOfEvents = this.GetEventsFromFile(file);
                result.AddRange(listOfEvents);
            }

            return Task.FromResult<IEnumerable<BaseItemEvent>>(result);
        }

        public Task<IEnumerable<BaseItemEvent>> GetAllForAggregateAsync(Guid aggregateId)
        {
            var filePath = Path.Combine(pathToEventsStoreFolder, String.Format(fileNameFormat, aggregateId));
            if (!File.Exists(filePath))
            {
                return Task.FromResult<IEnumerable<BaseItemEvent>>(new BaseItemEvent[0]);
            }

            var listOfEvents = this.GetEventsFromFile(filePath);
            return Task.FromResult(listOfEvents);
        }

        private IEnumerable<BaseItemEvent> GetEventsFromFile(string fileName)
        {
            var result = new List<BaseItemEvent>();
            using (var streamReader = new StreamReader(fileName))
            {
                while (!streamReader.EndOfStream)
                {
                    var serializedEvent = streamReader.ReadLine();
                    var evt = JsonConvert.DeserializeObject<BaseItemEvent>(serializedEvent, this.serializerSettings);
                    result.Add(evt);
                }
            }

            return result;
        }
    }
}
