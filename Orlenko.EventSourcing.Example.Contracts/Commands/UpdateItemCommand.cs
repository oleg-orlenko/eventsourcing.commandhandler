using Orlenko.EventSourcing.Example.Domain;
using System;

namespace Orlenko.EventSourcing.Example.Contracts.Commands
{
    public class UpdateItemCommand : BaseItemCommand
    {
        public readonly Item Item;

        public UpdateItemCommand(Item item, string userName) : base(userName)
        {
            this.Item = item ?? throw new ArgumentNullException(nameof(item));
        }
    }
}
