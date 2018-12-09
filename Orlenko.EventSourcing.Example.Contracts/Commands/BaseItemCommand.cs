using Orlenko.EventSourcing.Example.Contracts.Models;
using System;

namespace Orlenko.EventSourcing.Example.Contracts.Commands
{
    public abstract class BaseItemCommand
    {
        public readonly ItemModel Item;

        public readonly string UserName;

        public BaseItemCommand(ItemModel item, string userName)
        {
            Item = item ?? throw new ArgumentNullException(nameof(item));
            if (String.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            UserName = userName;
        }
    }
}
