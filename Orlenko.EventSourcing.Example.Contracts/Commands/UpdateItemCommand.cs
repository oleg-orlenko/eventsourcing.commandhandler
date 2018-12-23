using Orlenko.EventSourcing.Example.Contracts.Models;
using System;

namespace Orlenko.EventSourcing.Example.Contracts.Commands
{
    public class UpdateItemCommand : BaseItemCommand
    {
        public readonly ItemModel Item;

        public UpdateItemCommand(ItemModel item, string userName) : base(userName)
        {
            Item = item ?? throw new ArgumentNullException(nameof(item));
        }
    }
}
