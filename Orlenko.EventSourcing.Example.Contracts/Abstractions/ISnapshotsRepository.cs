using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Contracts.Abstractions
{
    public interface ISnapshotsRepository<TModel>
    {
        Task<TModel> GetAsync(Guid id);

        Task<IEnumerable<TModel>> GetByNameAsync(string name);

        Task SaveAsync(TModel snapshot);
    }
}
