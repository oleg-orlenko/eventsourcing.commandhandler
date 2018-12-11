using Orlenko.EventSourcing.Example.Contracts.Events;
using System;

namespace Orlenko.EventSourcing.Example.Contracts.Models
{
    public class ItemAggregate : BaseAggregate
    {
        public string Name { get; private set; }

        public ItemAggregate(Guid id) : base(id)
        {
        }

        public override AggregateApplicationResult ApplyEvent(BaseEvent evt)
        {
            if (evt == null)
            {
                throw new ArgumentNullException(nameof(evt));
            }

            switch (evt)
            {
                case ItemUpdatedEvent updated:
                    Name = updated.Name;
                    break;
            }

            return base.ApplyEvent(evt);
        }

        public override void Rollback()
        {
            // Have no idea at the moment what to do here ((
            base.Rollback();
        }
    }
}
