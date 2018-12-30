using Orlenko.EventSourcing.Example.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Contracts.Abstractions
{
    public interface IAggregateRepository<TAgg> where TAgg : BaseAggregate
    {
        Task<TAgg> GetByIdAsync(Guid id);

        Task<IEnumerable<TAgg>> GetByNameAsync(string name);

        Task CommitChangesAsync(TAgg aggregate);
    }
}
