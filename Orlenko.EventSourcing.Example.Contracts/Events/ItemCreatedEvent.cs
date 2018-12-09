using System;

namespace Orlenko.EventSourcing.Example.Contracts.Events
{
    public class ItemCreatedEvent : BaseItemEvent
    {
        // This is basically a payload, but I decided to use only string for example
        public readonly string Name;

        // Id is here just for deserialization purposes
        public ItemCreatedEvent(Guid id, string name, string userName, DateTime eventDate) : base(userName, id, eventDate)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));    
            }

            this.Name = name;
        }
    }
}
