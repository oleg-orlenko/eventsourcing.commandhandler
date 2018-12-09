using Orlenko.EventSourcing.Example.Contracts.Models;
using System;

namespace Orlenko.EventSourcing.Example.Contracts.Commands
{
    public class CreateItemCommand : BaseItemCommand
    {
        public CreateItemCommand(ItemModel item, string userName) : base(item, userName)
        {
        }
    }
}
