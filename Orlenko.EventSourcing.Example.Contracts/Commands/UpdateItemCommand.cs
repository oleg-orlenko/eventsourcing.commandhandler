using Orlenko.EventSourcing.Example.Contracts.Models;

namespace Orlenko.EventSourcing.Example.Contracts.Commands
{
    public class UpdateItemCommand : BaseItemCommand
    {
        public UpdateItemCommand(ItemModel item, string userName) : base(item, userName)
        {
        }
    }
}
