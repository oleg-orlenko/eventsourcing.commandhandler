using System;

namespace Orlenko.EventSourcing.Example.Domain
{
    public class Item : Entity
    {
        public string Name { get; private set; }

        public Item(Guid id, string name) : base(id)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public virtual void ChangeName(string newName)
        {
            if (newName is null)
                throw new ArgumentNullException(nameof(newName));

            Name = newName;
        }
    }
}
