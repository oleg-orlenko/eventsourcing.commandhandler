using Orlenko.EventSourcing.Example.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Orlenko.EventSourcing.Example.Domain
{
    public class ItemsList : GenericCollection<Item>
    {
        public ItemsList(IEnumerable<Item> items) : base(items)
        {
        }

        public override bool Add(Item item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (this.list.Any(x => x.Name.Equals(item.Name))) // Domain requirement is to have a unique constraint on the name
                return false;

            return base.Add(item);
        }

        public override IEnumerable<BaseEvent<Item>> GetChangesAfter(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
