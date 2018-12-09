using Orlenko.EventSourcing.Example.Contracts.Events;
using Orlenko.EventSourcing.Example.Contracts.Models;
using System;

namespace Orlenko.EventSourcing.Example.Core.Aggregates
{
    public class ItemAggregate : BaseAggregate
    {
        public string Name { get; private set; }

        public ItemAggregate(Guid id) : base(id)
        {
        }

        public override bool ApplyEvent(BaseEvent evt)
        {
            if (evt == null)
            {
                throw new ArgumentNullException(nameof(evt));    
            }

            switch(evt)
            {
                case ItemUpdatedEvent updated:
                    this.Name = updated.Name;
                    break;
            }

            return base.ApplyEvent(evt);
        }
    }
}
