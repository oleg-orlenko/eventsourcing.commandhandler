using System;

namespace Orlenko.EventSourcing.Example.Contracts.Events
{
    public class ItemCreatedEvent : BaseItemEvent
    {
        // This is basically a payload, but I decided to use only string for example
        public readonly string Name;

        public ItemCreatedEvent(Guid itemId, string name, string userName, Guid eventId, DateTime eventDate, int version = 0) : base(userName, itemId, eventId, eventDate, version)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));    
            }

            this.Name = name;
        }
    }
}
