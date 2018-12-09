using System;

namespace Orlenko.EventSourcing.Example.Contracts.Events
{
    public class ItemDeletedEvent : BaseItemEvent
    {
        public ItemDeletedEvent(Guid id, string userName, DateTime eventDate) : base(userName, id, eventDate)
        {
        }
    }
}
