using System;

namespace Orlenko.EventSourcing.Example.Contracts.Models
{
    public class ItemModel
    {
        public readonly string Name;

        public readonly Guid Id;

        public ItemModel(Guid id, string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Name = name;
            this.Id = id;
        }
    }
}
