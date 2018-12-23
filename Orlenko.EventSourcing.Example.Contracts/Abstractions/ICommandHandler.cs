using Orlenko.EventSourcing.Example.Contracts.Commands;
using Orlenko.EventSourcing.Example.Contracts.Events;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Contracts.Abstractions
{
    public interface ICommandHandler
    {
        Task<BaseItemEvent> HandleAsync(BaseItemCommand command);
    }
}


