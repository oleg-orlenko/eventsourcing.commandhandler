using Orlenko.EventSourcing.Example.Contracts.Commands;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Contracts.Abstractions
{
    public interface ICommandHandler
    {
        Task HandleAsync(BaseItemCommand command);
    }
}


