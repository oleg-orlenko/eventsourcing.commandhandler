using Orlenko.EventSourcing.Example.Contracts.Models;
using System;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Contracts.Abstractions
{
    public interface IAggregateRepository
    {
        Task<BaseAggregate> GetByIdAsync(Guid id);

        Task<bool> ExistsAsync(string name);

        Task CreateAsync(BaseAggregate aggregate);

        Task DeleteAsync(BaseAggregate aggregate);
    }
}
