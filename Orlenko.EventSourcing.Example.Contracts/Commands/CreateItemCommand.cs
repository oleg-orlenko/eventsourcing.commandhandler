using Orlenko.EventSourcing.Example.Contracts.Models;
using System;

namespace Orlenko.EventSourcing.Example.Contracts.Commands
{
    public class CreateItemCommand : BaseItemCommand
    {
        public readonly ItemModel Item;

        public CreateItemCommand(ItemModel item, string userName) : base(userName)
        {
            Item = item ?? throw new ArgumentNullException(nameof(item));
        }
    }
}
