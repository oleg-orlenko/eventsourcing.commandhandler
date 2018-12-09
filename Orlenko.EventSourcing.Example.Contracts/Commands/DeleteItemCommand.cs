using Orlenko.EventSourcing.Example.Contracts.Models;

namespace Orlenko.EventSourcing.Example.Contracts.Commands
{
    public class DeleteItemCommand : BaseItemCommand
    {
        public DeleteItemCommand(ItemModel item, string userName) : base(item, userName)
        {
        }
    }
}
