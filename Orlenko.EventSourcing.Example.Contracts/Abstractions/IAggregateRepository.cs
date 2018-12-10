using Orlenko.EventSourcing.Example.Contracts.Models;
using System;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Contracts.Abstractions
{
    public interface IAggregateRepository<TAgg> where TAgg : BaseAggregate
    {
        Task<TAgg> GetByIdAsync(Guid id);

        Task<bool> ExistsAsync(string name);

        Task CommitChangesAsync(TAgg aggregate);
    }
}
