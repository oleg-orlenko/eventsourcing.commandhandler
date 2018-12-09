using System;

namespace Orlenko.EventSourcing.Example.Contracts.Events
{
    public class BaseItemEvent : BaseEvent
    {
        public readonly string UserName;

        public BaseItemEvent(string userName, Guid id, DateTime eventDate) : base(id, eventDate)
        {
            this.UserName = userName;
        }
    }
}
