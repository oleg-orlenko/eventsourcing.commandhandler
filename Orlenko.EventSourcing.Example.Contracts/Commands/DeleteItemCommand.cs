using System;

namespace Orlenko.EventSourcing.Example.Contracts.Commands
{
    public class DeleteItemCommand : BaseItemCommand
    {
        public readonly Guid ItemId;

        public DeleteItemCommand(Guid itemId, string userName) : base(userName)
        {
            this.ItemId = itemId;
        }
    }
}
